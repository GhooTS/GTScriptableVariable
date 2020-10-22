using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GTVariable
{
    /// <summary>
    /// Zero argument event trigger
    /// </summary>
    public class EventTrigger : EventTriggerBase
    {
        [System.Serializable]
        public class Trigger
        {
            public GameEvent gameEvent;
            public UnityEventType eventType;

            public void Raise()
            {
                gameEvent.Raise();
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