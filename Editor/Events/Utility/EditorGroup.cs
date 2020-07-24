using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    public class EditorGroup<T> : UnityEditor.Editor
    where T : MonoBehaviour, new()
    {

        public bool FirstComponent { get; private set; } = false;

        private readonly static List<EditorGroupElement<T>> elements = new List<EditorGroupElement<T>>();
        private static bool allComponentsAttached = true;
        private static bool firstComponentEnabled = false;

        private bool showMixedValueForEnabled = false;
        private bool allComponentsEnabled = true;

        GameObject gameObject;
        public T TargetComponent { get; private set; }


        public override void OnInspectorGUI()
        {
            if (FirstComponent)
            {
                DrawOptions();
                DrawEditors();
            }
            else
            {
                if (GUILayout.Button("Attach"))
                {
                    var thisElement = elements.Find((element) => element.Component == TargetComponent);
                    thisElement.ShouldBeDetach = false;
                    thisElement.SetAttach(true);
                    EditorGUIUtility.PingObject(gameObject);
                }
                EditorGUILayout.Space();
                DrawDefaultInspector();
            }
        }

        protected void Init()
        {
            TargetComponent = target as T;
            if (TargetComponent != null)
            {
                gameObject = TargetComponent.gameObject;
                AttachComponents();
            }
        }

        protected void AttachComponents()
        {
            FirstComponent = TargetComponent == gameObject.GetComponent<T>();


            if (FirstComponent && firstComponentEnabled == false)
            {
                Undo.undoRedoPerformed += Refresh;
                allComponentsAttached = true;
                firstComponentEnabled = true;
            }

            if (FirstComponent || (FirstComponent == false && allComponentsAttached == false))
            {
                Refresh();
            }
        }

        protected void DetachComponents()
        {
            if (UnityEditor.Selection.activeGameObject != gameObject)
            {
                Undo.undoRedoPerformed -= Refresh;
                allComponentsAttached = false;
                SetVisiableFlag(allComponentsAttached);
                elements.Clear();
                firstComponentEnabled = false;
            } 

            if (FirstComponent && gameObject != null)
            {
                MoveToNextComponent();
            }
        }

        protected virtual void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();

            var showMixedValue = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = showMixedValueForEnabled;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Toggle(allComponentsEnabled, GUILayout.MaxWidth(18));

            if (EditorGUI.EndChangeCheck()) 
            {
                allComponentsEnabled = !allComponentsEnabled;
                SetEnabledForAllComponents(allComponentsEnabled);
                showMixedValueForEnabled = IsMixedEnabled();
            }

            EditorGUI.showMixedValue = showMixedValue;

            if (FirstComponent && GUILayout.Button("Add", EditorStyles.miniButton))
            {
                AttachComponent(Undo.AddComponent<T>(gameObject));
            }

            if (GUILayout.Button("Remove All", EditorStyles.miniButton))
            {
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(elements[i].Component);
                    elements.RemoveAt(i);
                }
            }

            if (GUILayout.Button(allComponentsAttached ? "Detach All" : "Attach All",EditorStyles.miniButton))
            {
                allComponentsAttached = !allComponentsAttached;
                SetVisiableFlag(allComponentsAttached);
                EditorGUIUtility.PingObject(gameObject); //TODO: better way to update inspector
            }

            EditorGUILayout.LabelField($"Attach: {elements.Count}", GUILayout.MaxWidth(60));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        protected virtual void DrawEditor(SerializedObject serializedObject)
        {
            if (serializedObject == null) return;

            var property = serializedObject.GetIterator();
            property.NextVisible(true);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(property);
            GUI.enabled = true;
            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        protected virtual void DrawEditors()
        {
            

            for (int i = 0; i < elements.Count; i++)
            {
                var foldoutContent = new GUIContent(elements[i].Name);

                
                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.Toggle(elements[i].Enabled, GUILayout.MaxWidth(18));

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(elements[i].Component, "Component Enabled");
                    elements[i].Enabled = !elements[i].Enabled;
                    showMixedValueForEnabled = IsMixedEnabled();
                    if (showMixedValueForEnabled == false) allComponentsEnabled = elements[0].Enabled;
                }

                GUILayout.Space(10);

                elements[i].Foldout = EditorGUILayout.BeginFoldoutHeaderGroup(elements[i].Foldout, foldoutContent, null,
                                (position) =>
                                {
                                    var menu = new GenericMenu();
                                    if (i != 0 && elements[i].Attach)
                                    { 
                                        menu.AddItem(new GUIContent("Detach"), false, DetachComponent, i); 
                                    }
                                    menu.AddItem(new GUIContent("Remove"), false, RemoveComponent, i);
                                    menu.DropDown(position);
                                });

                EditorGUILayout.EndHorizontal(); 

                
                if (elements[i].Foldout)
                {
                    EditorGUI.BeginChangeCheck();

                    elements[i].Update();
                    DrawEditor(elements[i].SerializedObject);
                    elements[i].ApplyModifiedProperties();

                    if (EditorGUI.EndChangeCheck())
                    {
                        elements[i].SetName(GetComponentName(elements[i].Component));
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        protected virtual string GetComponentName(T component)
        {
            return string.Empty;
        }

        private void Refresh()
        {
            
            var attachComponents = gameObject.GetComponents<T>();
            if (elements.Count != attachComponents.Length)
            {
                for (int i = 0; i < attachComponents.Length; i++)
                {
                    if(i < elements.Count)
                    {
                        elements[i].ChangeComponent(attachComponents[i], GetComponentName(attachComponents[i]));
                    }
                    else
                    {
                        elements.Add(new EditorGroupElement<T>(attachComponents[i], GetComponentName(attachComponents[i])));
                    }
                }

                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    if(elements[i].HasComponent() == false)
                    {
                        elements.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i].ChangeComponent(attachComponents[i],GetComponentName(attachComponents[i]));
                }
            }
            SetVisiableFlag(allComponentsAttached);

            showMixedValueForEnabled = IsMixedEnabled();
            if (showMixedValueForEnabled == false && elements.Count != 0)
            {
                allComponentsEnabled = elements[0].Enabled;
            }
        }

        private void MoveToNextComponent()
        {
            if (elements.Count == 0) return;

            if (TargetComponent == null)
            {
                if (elements.First().HasComponent() == false)
                {
                    elements.RemoveAt(0);
                    return;
                }

                elements.First().SetAttach(false);
            }
        }

        private void SetVisiableFlag(bool visiable)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].SetAttach(i != 0 && visiable); //Always false for first element
            }
        }

        private void AttachComponent(T component)
        {
            elements.Add(new EditorGroupElement<T>(component));
            elements.Last().SetAttach(allComponentsAttached);
        }

        private void RemoveComponent(object index)
        {
            int id = (int)index;
            elements[id].SetAttach(false);
            Undo.DestroyObjectImmediate(elements[id].Component);
            elements.RemoveAt(id);
        }

        private void DetachComponent(object index)
        {
            int id = (int)index;
            elements[id].ShouldBeDetach = true;
            elements[id].SetAttach(false);
            EditorGUIUtility.PingObject(gameObject);
        }

        private bool IsMixedEnabled()
        {
            if (elements.Count < 2) return false;

            var componentEnabled = elements[0].Enabled;

            for (int i = 1; i < elements.Count; i++)
            {
                if(elements[i].Enabled != componentEnabled)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetEnabledForAllComponents(bool enabled)
        {
            Undo.RecordObjects(elements.ConvertAll((element) => element.Component).ToArray(),"Components Enabled");
            elements.ForEach((element) => element.Enabled = enabled);
        }

    }
}