using UnityEditor;
using UnityEngine;

using GTVariable.Editor.Utility;

namespace GTVariable.Editor.Debugging
{
    public class EditorGroupDebugWindow : EditorWindow
    {
        private MonoBehaviour[] components;
        private MonoBehaviour[] prevComponents;
        private Vector2 scrollVector;

        [MenuItem("Window/ScriptableVars/Debug/Editor Group Debug Window")]
        public static void Init()
        {
            GetWindow<EditorGroupDebugWindow>("Editor Group Debug Window");
        }

        private void OnEnable()
        {
            position = WindowPrefs.LoadPosition(titleContent.text);
            Selection.selectionChanged += UpdateComponentsList;
            
        }

        private void OnDisable()
        {
            WindowPrefs.SavePosition(titleContent.text, position);
            Selection.selectionChanged -= UpdateComponentsList;
        }


        private void OnInspectorUpdate()
        {
            if (Selection.activeGameObject != null)
            {
                components = Selection.activeGameObject.GetComponents<MonoBehaviour>();
                Repaint();
            }
            else
            {
                components = null;
            }
        }



        private void OnGUI()
        {
            if(Selection.activeGameObject != null && components != null)
            {
                scrollVector = EditorGUILayout.BeginScrollView(scrollVector);
                EditorGUILayout.LabelField("Current Selected");
                foreach (var component in components)
                {
                    if (component == null) continue;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(component.GetType().Name);
                    EditorGUILayout.LabelField(component.hideFlags.ToString());
                    EditorGUILayout.EndHorizontal();
                }

                if (prevComponents != null)
                {
                    EditorGUILayout.LabelField("Preview Selected");
                    foreach (var component in prevComponents)
                    {
                        if (component == null) continue;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(component.GetType().Name);
                        EditorGUILayout.LabelField(component.hideFlags.ToString());
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("Select gameobject");
            }
        }


        private void UpdateComponentsList()
        {
            if (Selection.activeGameObject != null)
            {
                if (components != null) prevComponents = components;
                components = Selection.activeGameObject.GetComponents<MonoBehaviour>();
                Repaint();
            }
            else
            {
                components = null;
            }
        }
    }
}