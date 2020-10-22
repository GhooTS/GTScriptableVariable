using UnityEditor;

namespace GTVariable.Editor
{
    /// <summary>
    /// Derive from this class to create editor for one argument event trigger
    /// </summary>
    public class EventTriggerEditor<GameEventType, ListenerType, EventType, ParameterType> : EventTriggerEditor
           where GameEventType : ParameterizedGameEvent<ParameterizedListener<GameEventType, EventType, ParameterType>, EventType, ParameterType>
           where ListenerType : ParameterizedListener<GameEventType, EventType, ParameterType>
           where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        protected override void DrawTrigger()
        {
            base.DrawTrigger();
            property = trigger.FindPropertyRelative("parameter");
            EditorGUILayout.PropertyField(property);
        }
    }

}