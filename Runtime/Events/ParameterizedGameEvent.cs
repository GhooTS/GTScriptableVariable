using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{
    public abstract class ParameterizedGameEvent<ListenerType, EventType, ParameterType> : ScriptableObject
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where ListenerType : IRaisable<EventType, ParameterType>
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