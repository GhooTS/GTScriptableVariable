using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace GTVariable.Editor
{
    /// <summary>
    /// Derive from this class to create editor for custom listener
    /// </summary>
    [CustomEditor(typeof(GameEventListener))]
    internal class ListenerEditor : EditorGroup<Listener>
    {
        SerializedProperty listenerName;
        SerializedProperty listenerDescription;
        SerializedProperty gameEvents;
        SerializedProperty responses;
        GUIContent plusIcon;
        GUIContent minusIcon;
        GUIStyle plusButtonStyle; 
        GUIStyle gameEventStyle;
        GUIStyle headerTextStyle; 

        private void OnEnable()
        {
            Init();
            plusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"));
            minusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            plusButtonStyle = new GUIStyle();
            plusButtonStyle.imagePosition = ImagePosition.ImageOnly;
            plusButtonStyle.padding = new RectOffset(0, 0, 3, 0);

            gameEventStyle = new GUIStyle();
            gameEventStyle.padding = new RectOffset(10, 10, 10, 10);
            headerTextStyle = null;
        }

        private void OnDisable()
        {
            DetachComponents();
        }

        protected override void DrawEditor(int index,SerializedObject serializedObject)
        {
            
            if(headerTextStyle == null)
            {
                headerTextStyle = new GUIStyle(EditorStyles.label);
                headerTextStyle.alignment = TextAnchor.MiddleLeft;
                headerTextStyle.padding = new RectOffset(5, 0, 0, 2);
            }


            listenerName = serializedObject.FindProperty("listenerName");
            listenerDescription = serializedObject.FindProperty("listenerDescription");
            gameEvents = serializedObject.FindProperty("gameEvents");
            responses = serializedObject.FindProperty("response");

            var content = new GUIContent("Name");
            EditorGUILayout.PropertyField(listenerName,content);
            content.text = "Description";
            EditorGUILayout.PropertyField(listenerDescription,content);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal("RL Header");
            EditorGUILayout.LabelField(gameEvents.displayName, headerTextStyle);
            if (GUILayout.Button(plusIcon, plusButtonStyle,GUILayout.MaxWidth(20)))
            {
                gameEvents.InsertArrayElementAtIndex(gameEvents.arraySize);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("RL Background");
            EditorGUILayout.BeginVertical(gameEventStyle);
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
                    var deleteTwice = gameEvents.GetArrayElementAtIndex(i).objectReferenceValue != null;
                    gameEvents.DeleteArrayElementAtIndex(i);
                    if (deleteTwice) gameEvents.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
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