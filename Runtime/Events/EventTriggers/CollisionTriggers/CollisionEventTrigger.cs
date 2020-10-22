using UnityEngine;
using System.Collections.Generic;

namespace GTVariable
{
    /// <summary>
    /// Zero argument collision event trigger
    /// </summary>
    public class CollisionEventTrigger : CollisionEventTriggerBase
    {
        [System.Serializable]
        public class Trigger
        {
            public bool any;
            public string tag;
            public GameEvent gameEvent;
            public PhysicEventType eventType;
        }

        public List<Trigger> triggers = new List<Trigger>();

        protected override void TriggerEvent(PhysicEventType eventType, GameObject gameObject)
        {
            foreach (var trigger in triggers)
            {
                if(eventType == trigger.eventType && (trigger.any || gameObject.CompareTag(trigger.tag)))
                {
                    trigger.gameEvent.Raise();
                }
            }
        }
    }
}