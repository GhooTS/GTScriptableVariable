using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

namespace GTVariable
{
    public abstract class GameEvent<EventType, ParameterType> : GameEventBase
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        /// <summary>
        /// This action is invoke whenever game event is Raise 
        /// </summary>
        public UnityAction<ParameterType> OnEventRaisedWithParameter { get; set; }

        public void Raise(ParameterType value)
        {
            OnEventRaised?.Invoke();
            OnEventRaisedWithParameter?.Invoke(value);
        }
    }
}