using UnityEngine;
using System.Collections.Generic;

namespace GTVariable
{

    /// <summary>
    /// Derive from this class to create collision event trigger for one argument game event
    /// </summary>
    public class CollisionEventTrigger<GameEventType, ListenerType, EventType, ParameterType> : CollisionEventTriggerBase
        where GameEventType : GameEvent<Listener<GameEventType, EventType, ParameterType>, EventType, ParameterType>
        where ListenerType : Listener<GameEventType, EventType, ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        [System.Serializable]
        public class Trigger
        {
            public bool any;
            public string tag;
            public GameEventType gameEvent;
            public PhysicEventType eventType;
            public ParameterType parameter;
            public bool dynamicParameter = false;
            public TriggerBehaviour behaviour = TriggerBehaviour.StopOnNull;

            public void Raise()
            {
                gameEvent.Raise(parameter);
            }

            public void Raise(ParameterType parameter)
            {
                gameEvent.Raise(parameter);
            }
        }

        public enum TriggerBehaviour
        {
            StopOnNull,
            TriggerWithDefault,
            TriggerWithNull
        }

        public List<Trigger> triggers = new List<Trigger>();

        protected override void TriggerEvent(PhysicEventType eventType,GameObject gameObject)
        {
            foreach (var trigger in triggers)
            {
                if (eventType == trigger.eventType && (trigger.any || gameObject.CompareTag(trigger.tag)))
                {
                    if (trigger.dynamicParameter)
                    {
                        if(gameObject.TryGetComponent(out ParameterType component) || trigger.behaviour == TriggerBehaviour.TriggerWithNull)
                        {
                            trigger.Raise(component);
                        }
                        else if(trigger.behaviour == TriggerBehaviour.TriggerWithDefault)
                        {
                            trigger.Raise();
                        }

                    }
                    else
                    {
                        trigger.Raise();
                    }
                }
            }
        }
    }
}