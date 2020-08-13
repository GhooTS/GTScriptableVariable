using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(GameEventListener))]
    public class GameEventListenerEditor : EditorGroup<Listener>
    {
        SerializedProperty listenerName;
        SerializedProperty listenerDescription;
        SerializedProperty gameEvents;
        SerializedProperty responses;
        GUIContent plusIcon;
        GUIContent minusIcon;
        GUIStyle plusButtonStyle; 

        private void OnEnable()
        {
            Init();
            plusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"));
            minusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            plusButtonStyle = new GUIStyle();
            plusButtonStyle.imagePosition = ImagePosition.ImageOnly;
            plusButtonStyle.padding = new RectOffset(0, 0, 3, 0);
        }

        private void OnDisable()
        {
            DetachComponents();
        }

        protected override void DrawEditor(int index,SerializedObject serializedObject)
        {
            

            listenerName = serializedObject.FindProperty("listenerName");
            listenerDescription = serializedObject.FindProperty("listenerDescription");
            gameEvents = serializedObject.FindProperty("gameEvents");
            responses = serializedObject.FindProperty("response");

            var content = new GUIContent("Name");
            EditorGUILayout.PropertyField(listenerName,content);
            content.text = "Description";
            EditorGUILayout.PropertyField(listenerDescription,content);

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(gameEvents.displayName);
            if (GUILayout.Button(plusIcon, plusButtonStyle,GUILayout.MaxWidth(20)))
            {
                gameEvents.InsertArrayElementAtIndex(gameEvents.arraySize);
            }

            EditorGUILayout.EndHorizontal();
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;
            if(gameEvents.arraySize == 0)
            {
                EditorGUILayout.LabelField("List is Empty");
            }
            for (int i = 0; i < gameEvents.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(EditorGUIUtility.singleLineHeight + 3));
                EditorGUILayout.PropertyField(gameEvents.GetArrayElementAtIndex(i),GUIContent.none);
                if (GUILayout.Button(minusIcon, plusButtonStyle, GUILayout.MaxWidth(20)))
                {
                    gameEvents.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel = indentLevel;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(responses);
        }

        protected override string GetComponentName(Listener component)
        {
            return component.listenerName;
        }


    }
}