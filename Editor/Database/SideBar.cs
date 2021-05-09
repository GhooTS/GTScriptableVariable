using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System;

using Object = UnityEngine.Object;
using System.Reflection;

namespace GTVariable.Editor
{
    public class SideBar
    {
        public SerializedProperty Array { get; set; }
        public Type ElementType { get; set; }
        public SerializedObject SerializedObject { get { return Array.serializedObject; } }
        public int Selected { get; private set; }
        public Object CurrentSelected { get { return Selected == -1 || Selected >= Array.arraySize ? null : Array.GetArrayElementAtIndex(Selected).objectReferenceValue; } }
        public bool CanDuplicated { get; set; } = true;
        public bool CanDelete { get; set; } = true;
        public bool CanCreate { get; set; } = true;

        public event Action<Object> OnElementDelete;

        private string searchText;
        private SearchField searchField;
        private ComponenetCreator componenetCreator = new ComponenetCreator();

        private Vector2 scrollVector;

        public void OnGUI()
        {
            if (searchField == null) searchField = new SearchField();


            //Draw header
            using (var scope = new EditorGUILayout.HorizontalScope(GTGUIStyles.sideBarHeader))
            {
                EditorGUILayout.LabelField(Array.displayName, EditorStyles.boldLabel);
                if (GUILayout.Button(GTGUIStyles.PlusIcon, GTGUIStyles.plusButtonStyle, GUILayout.MaxWidth(20)) && CanCreate)
                {
                    componenetCreator.UpdateComponentsTypesList(ElementType);
                    componenetCreator.ShowObjectMenu(OnTypeSelected);
                }
            }

            //Draw content
            EditorGUILayout.BeginVertical(GTGUIStyles.sideBarContent);
            scrollVector = EditorGUILayout.BeginScrollView(scrollVector);
            if (Array.arraySize == 0)
            {
                EditorGUILayout.LabelField("List is Empty", GTGUIStyles.labelCenter);
            }

            for (int i = 0; i < Array.arraySize; i++)
            {
                var current = Array.GetArrayElementAtIndex(i);

                

                using (var scope = new EditorGUILayout.HorizontalScope(Selected == i ? GTGUIStyles.rowSelected : GTGUIStyles.row))
                {

                    EditorGUILayout.LabelField(GetElementName(i));
                    
                    GUI.Label(scope.rect, GUIContent.none);
                    Rect rect = scope.rect;
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.MouseDown)
                        {
                            if (Event.current.button == 0)
                            {
                                GUI.FocusControl("");
                                Selected = i;
                                Event.current.Use();
                            }
                            else if (Event.current.button == 1)
                            {
                                var menu = new GenericMenu();
                                if(CanDelete) menu.AddItem(new GUIContent("Delete"), false,() => OnDelete(current));
                                if(CanDuplicated) menu.AddItem(new GUIContent("Duplicated"), false, () => OnDuplicated(current));
                                menu.ShowAsContext();
                            }
                        }
                    }
                }

                var lineRect = GUILayoutUtility.GetRect(GUIContent.none, GTGUIStyles.sideBarElementLine);
                GUI.Box(lineRect, GUIContent.none);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            Selected = Array.arraySize == 0 ? -1 : Mathf.Clamp(Selected, 0, Array.arraySize);
        }

        protected virtual void OnDelete(SerializedProperty target)
        {
            var objRef = target.objectReferenceValue;
            if (objRef == null || EditorUtility.DisplayDialog("Delete Selected Asset?", "You cannot undo this action.", "Delete", "Cancel"))
            {
                SerializedObject.Update();


                var index = GTGUIUtilites.FindIndexInProperty(target.objectReferenceValue, Array);
                var deleteTwice = target.objectReferenceValue != null;
                Array.DeleteArrayElementAtIndex(index);
                if (deleteTwice) Array.DeleteArrayElementAtIndex(index);
                SerializedObject.ApplyModifiedProperties();
                OnElementDelete.Invoke(objRef);
                Selected = Array.arraySize == 0 ? -1 : Mathf.Clamp(Selected, 0, Array.arraySize);
            }
        }

        protected virtual void OnDuplicated(SerializedProperty target)
        {
            SerializedObject.Update();
            var index = GTGUIUtilites.FindIndexInProperty(target.objectReferenceValue, Array);
            Array.InsertArrayElementAtIndex(index);
            Array.GetArrayElementAtIndex(index).objectReferenceValue = target.objectReferenceValue;
            SerializedObject.ApplyModifiedProperties();
            Selected = Array.arraySize == 0 ? -1 : Mathf.Clamp(Selected, 0, Array.arraySize);
        }

        public virtual string GetElementName(int i)
        {
            if (Array == null) return string.Empty;

            var objRef = Array.GetArrayElementAtIndex(i).objectReferenceValue;

            return objRef != null ? objRef.name : string.Empty;
        }


        private void OnTypeSelected(object type)
        {
            SerializedObject.Update();

            Array.InsertArrayElementAtIndex(Array.arraySize);

            var newObject = ScriptableObject.CreateInstance((Type)type) as Object;

            var nameable = newObject as INameable;

            if (nameable != null)
            {
                nameable.Name = newObject.name = $"New{newObject.GetType().Name}";
            }
            else
            {
                newObject.name = $"New{newObject.GetType().Name}";
            }

            AssetDatabase.AddObjectToAsset(newObject, SerializedObject.targetObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Array.GetArrayElementAtIndex(Array.arraySize - 1).objectReferenceValue = newObject;

            SerializedObject.ApplyModifiedProperties();

        }

    }
}