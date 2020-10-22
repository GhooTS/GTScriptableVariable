using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor
{

    /// <summary>
    /// Derive from this class to create editor for one argument event trigger,
    /// this class is also base class for all collision and normal event trigger editors
    /// </summary>
    [CustomEditor(typeof(EventTrigger))]
    public class EventTriggerEditor : UnityEditor.Editor
    {
        protected SerializedProperty triggers;
        protected SerializedProperty trigger;
        protected SerializedProperty property;

        private GUIContent minusIcon;
        private GUIStyle minusButtonStyle;
        private GUIStyle headerTextStyle;
        private GUIStyle triggerStyle;

        protected virtual void OnEnable()
        {
            triggers = serializedObject.FindProperty("triggers");
            minusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
           
            minusButtonStyle = new GUIStyle();
            minusButtonStyle.imagePosition = ImagePosition.ImageOnly;
            minusButtonStyle.padding = new RectOffset(0, 0, 3, 0);
            triggerStyle = new GUIStyle();
            triggerStyle.padding = new RectOffset(10, 10, 0, 0);
            headerTextStyle = null;
        }

        public override void OnInspectorGUI()
        {
            if(headerTextStyle == null)
            {
                headerTextStyle = new GUIStyle(EditorStyles.label);
                headerTextStyle.alignment = TextAnchor.MiddleLeft;
                headerTextStyle.padding = new RectOffset(5, 0, 0, 2);
            }

            serializedObject.Update();
            for (int i = 0; i < triggers.arraySize; i++)
            {
                trigger = triggers.GetArrayElementAtIndex(i);
                if (DrawTriggerHeader(i)) break;
                EditorGUILayout.BeginVertical("RL Background");
                EditorGUILayout.BeginVertical(triggerStyle);
                EditorGUILayout.Space(3f);
                DrawTrigger();
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            if(GUILayout.Button("Add new trigger"))
            {
                triggers.InsertArrayElementAtIndex(triggers.arraySize);
            }
            serializedObject.ApplyModifiedProperties();
        }

        private bool DrawTriggerHeader(int index)
        {
            EditorGUILayout.BeginHorizontal("RL Header");
            property = trigger.FindPropertyRelative("eventType");
            EditorGUILayout.LabelField(property.enumDisplayNames[property.enumValueIndex],headerTextStyle);
            if (GUILayout.Button(minusIcon, minusButtonStyle, GUILayout.MaxWidth(20f)))
            {
                triggers.DeleteArrayElementAtIndex(index);
                return true;
            }
            EditorGUILayout.EndHorizontal();
            return false;
        }

        protected virtual void DrawTrigger()
        {
            property = trigger.FindPropertyRelative("gameEvent");
            EditorGUILayout.PropertyField(property);
            property = trigger.FindPropertyRelative("eventType");
            EditorGUILayout.PropertyField(property);
        }
    }

}