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
    public class ListenerEditor : EditorGroup<Listener>
    {
        private SerializedProperty listenerName;
        private SerializedProperty listenerDescription;
        private SerializedProperty gameEvents;
        private SerializedProperty responses;

        private void OnEnable()
        {
            Init();
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
            GTGUILayout.ArrayProperty(gameEvents, "Game Events");
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(responses);
        }

        protected override string GetComponentName(Listener component)
        {
            return component.listenerName;
        }


    }
}