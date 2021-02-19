using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

namespace GTVariable
{
    public abstract class GameEvent<ListenerType, EventType, ParameterType> : GameEventBase
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where ListenerType : Listener,IListener<EventType, ParameterType>
    {
        /// <summary>
        /// List of listeners that subscirbe to this game event
        /// </summary>
        public List<ListenerType> EventListners { get { return eventListners; } }

        /// <summary>
        /// This action is invoke whenever game event is Raise 
        /// </summary>
        public UnityAction<ParameterType> OnEventRaise { get; private set; }
        protected readonly List<ListenerType> eventListners = new List<ListenerType>();

        public void Raise(ParameterType value)
        {
            OnEventRaise?.Invoke(value);
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