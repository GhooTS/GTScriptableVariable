using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace GTVariable
{
    public abstract class ParameterizedGameEvent<ListenerType, EventType, ParameterType> : GameEventBase
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where ListenerType : Listener,IParameterizedListener<EventType, ParameterType>
    {
        public List<ListenerType> EventListners { get { return eventListners; } }
        protected readonly List<ListenerType> eventListners = new List<ListenerType>();

        public void Raise(ParameterType value)
        {
            for (int i = eventListners.Count - 1; i >= 0; i--)
            {
                eventListners[i].OnEventRised(value);
            }
        }

        public override void RegisterListener(Listener listner)
        {
            //eventListners.Add(listner);
        }

        public override void UnRegisterListener(Listener listner)
        {
            //eventListners.Remove(listner);
        }

        public void RegisterListener(ListenerType listner)
        {
            eventListners.Add(listner);
        }

        public void UnRegisterListener(ListenerType listner)
        {
            eventListners.Remove(listner);
        }
    }
}