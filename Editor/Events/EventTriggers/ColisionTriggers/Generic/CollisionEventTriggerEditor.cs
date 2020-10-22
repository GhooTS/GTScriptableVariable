using UnityEditor;

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
            //Check if parameter type derive from Object and don't derive from Scritpable object
            dynamicParameterValid = typeof(UnityEngine.Object).IsAssignableFrom(typeof(ParameterType)) 
                                 && !typeof(UnityEngine.ScriptableObject).IsAssignableFrom(typeof(ParameterType));
        }


        protected override void DrawTrigger()
        {
            base.DrawTrigger();
            property = trigger.FindPropertyRelative("parameter");
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
            }
        }
    }
}