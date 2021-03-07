using System.Collections.Generic;


namespace GTVariable
{

    /// <summary>
    /// Derive from this class to create event trigger for one argument game event
    /// </summary>
    public class EventTrigger<GameEventType,EventType,ParameterType> : EventTriggerBase
        where GameEventType : GameEvent<EventType,ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        [System.Serializable]
        public class Trigger
        {
            public GameEventType gameEvent;
            public UnityEventType eventType;
            public ParameterType parameter;


            public void Raise()
            {
                gameEvent.Raise(parameter);
            }
        }

        public List<Trigger> triggers = new List<Trigger>();

        protected override void TriggerEvent(UnityEventType eventType)
        {
            foreach (var trigger in triggers)
            {
                if (eventType == trigger.eventType)
                {
                    trigger.Raise();
                }
            }

        }
    }
}