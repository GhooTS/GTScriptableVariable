using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor
{

    [CustomEditor(typeof(CollisionEventTrigger))]
    public class CollisionEventTriggerEditor : EventTriggerEditor
    {
        
        protected override void DrawTrigger()
        {
            property = trigger.FindPropertyRelative("any");
            EditorGUILayout.PropertyField(property);
            if (property.boolValue == false)
            {
                property = trigger.FindPropertyRelative("tag");
                property.stringValue = EditorGUILayout.TagField(property.displayName, property.stringValue);
            }
            property = trigger.FindPropertyRelative("gameEvent");
            EditorGUILayout.PropertyField(property);
            property = trigger.FindPropertyRelative("eventType");
            EditorGUILayout.PropertyField(property);
        }
    }
}