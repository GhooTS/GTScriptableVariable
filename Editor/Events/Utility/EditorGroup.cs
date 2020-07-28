using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    public class EditorGroup<T> : UnityEditor.Editor
        where T : MonoBehaviour
    {

        public bool FirstComponent { get; private set; } = false;
        public T TargetComponent { get; private set; }
        public GameObject GameObject { get; private set; }
        public int Count { get { return elements.Count; } }


        protected readonly ComponentEnabledGruopController<T> enabledGruop = new ComponentEnabledGruopController<T>();
        protected readonly ComponenetCreator<T> componenetCreator = new ComponenetCreator<T>();
        protected readonly static List<EditorGroupElement<T>> elements = new List<EditorGroupElement<T>>();
        private static bool allComponentsAttached = true;
        private static bool firstComponentEnabled = false;




        public override void OnInspectorGUI()
        {
            if (FirstComponent)
            {
                DrawOptions();
                DrawEditors();
            }
            else
            {
                if (allComponentsAttached && GUILayout.Button("Attach"))
                {
                    var thisElement = elements.Find((element) => element.Component == TargetComponent);
                    thisElement.ShouldBeDetach = false;
                    thisElement.SetAttach(true);
                    EditorGUIUtility.PingObject(GameObject);
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
                GameObject = TargetComponent.gameObject;
                AttachComponents();
                componenetCreator.UpdateComponentsTypesList();
            }
        }

        protected void AttachComponents()
        {
            FirstComponent = TargetComponent == GameObject.GetComponent<T>();


            if (FirstComponent && firstComponentEnabled == false)
            {
                Undo.undoRedoPerformed += Refresh;
                AssemblyReloadEvents.beforeAssemblyReload += DetachDefinitively;
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
            if (UnityEditor.Selection.activeGameObject != GameObject)
            {
                Undo.undoRedoPerformed -= Refresh;
                allComponentsAttached = false;
                SetVisiableFlag(allComponentsAttached);
                elements.Clear();
                firstComponentEnabled = false;
            } 

            if (FirstComponent && GameObject != null)
            {
                MoveToNextComponent();
            }
        }

        private void DetachDefinitively()
        {
            Undo.undoRedoPerformed -= Refresh;
            AssemblyReloadEvents.beforeAssemblyReload -= DetachDefinitively;
            allComponentsAttached = false;
            SetVisiableFlag(allComponentsAttached);
            elements.Clear();
            firstComponentEnabled = false;
        }

        protected virtual void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();

            var showMixedValue = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = enabledGruop.showMixedValue;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Toggle(enabledGruop.allEnabled, GUILayout.MaxWidth(18));

            if (EditorGUI.EndChangeCheck()) 
            {
                enabledGruop.SwitchEnabledForAllComponents(elements);
                enabledGruop.UpdateMixedEnabled(elements);
            }

            EditorGUI.showMixedValue = showMixedValue;

            if (FirstComponent && GUILayout.Button("Add", EditorStyles.miniButton))
            {
                componenetCreator.ShowComponentMenu(GUILayoutUtility.GetLastRect(),GameObject);
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
                EditorGUIUtility.PingObject(GameObject); //TODO: better way to update inspector
            }

            EditorGUILayout.LabelField($"Attach: {elements.Count}", GUILayout.MaxWidth(60));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        protected virtual void DrawEditor(int index,SerializedObject serializedObject)
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
                    enabledGruop.SetEnabled(i, elements);
                }

                GUILayout.Space(10);

                elements[i].Foldout = EditorGUILayout.BeginFoldoutHeaderGroup(elements[i].Foldout, foldoutContent,null,
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
                    DrawEditor(i,elements[i].SerializedObject);
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
            RemoveEmptyElements();
            var attachComponents = GameObject.GetComponents<T>();
            if (elements.Count != attachComponents.Length)
            {
                for (int i = 0; i < attachComponents.Length; i++)
                {
                    if (i < elements.Count)
                    {
                        elements[i].ChangeComponent(attachComponents[i], GetComponentName(attachComponents[i]));
                    }
                    else
                    {
                        elements.Add(new EditorGroupElement<T>(attachComponents[i], GetComponentName(attachComponents[i])));
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

            enabledGruop.UpdateMixedEnabled(elements);
        }

        private void RemoveEmptyElements()
        {
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                if (elements[i].HasComponent() == false)
                {
                    elements.RemoveAt(i);
                }
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
            EditorGUIUtility.PingObject(GameObject);
        }

    }
}

