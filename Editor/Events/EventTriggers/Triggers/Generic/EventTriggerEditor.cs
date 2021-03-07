using UnityEditor;

namespace GTVariable.Editor
{
    /// <summary>
    /// Derive from this class to create editor for one argument event trigger
    /// </summary>
    public class EventTriggerEditor<GameEventType, EventType, ParameterType> : EventTriggerEditor
           where GameEventType : GameEvent<EventType, ParameterType>
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