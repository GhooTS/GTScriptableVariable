using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{

    /// <summary>
    /// Derive from this class to create editor for one argument collision event trigger
    /// </summary>
    public class CollisionEventTriggerEditor<GameEventType, ListenerType, EventType, ParameterType> : CollisionEventTriggerEditor
        where GameEventType : GameEvent<Listener<GameEventType, EventType, ParameterType>, EventType, ParameterType>
        where ListenerType : Listener<GameEventType, EventType, ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {

        private bool dynamicParameterValid;

        protected override void OnEnable()
        {
            base.OnEnable();
            //Check if parameter type derive from Component
            dynamicParameterValid = typeof(UnityEngine.Component).IsAssignableFrom(typeof(ParameterType));
        }


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
            
            if (dynamicParameterValid)
            {
                property = trigger.FindPropertyRelative("dynamicParameter");
                EditorGUILayout.PropertyField(property);
                if (property.boolValue)
                {
                    property = trigger.FindPropertyRelative("behaviour");
                    EditorGUILayout.PropertyField(property);
                }
                else
                {
                    property = trigger.FindPropertyRelative("parameter");
                    EditorGUILayout.PropertyField(property);
                }
            }
            else
            {

            }
        }
    }
}