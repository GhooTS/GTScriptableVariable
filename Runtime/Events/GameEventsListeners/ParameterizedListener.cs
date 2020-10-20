using UnityEngine;

namespace GTVariable
{
    public abstract class ParameterizedListener<GameEventType,EventType,ParameterType> : Listener,IParameterizedListener<EventType,ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where GameEventType : ParameterizedGameEvent<ParameterizedListener<GameEventType, EventType, ParameterType>, EventType,ParameterType>
    {
        /// <summary>
        /// List of game events to which this listener subscribe to
        /// </summary>
        public GameEventType[] gameEvents;

        /// <summary>
        /// Response which will be call <seealso cref="OnEventRised(ParameterType)"/>
        /// </summary>
        public EventType response;

        /// <summary>
        /// Invoke <seealso cref="response"/> with specify value
        /// </summary>
        public void OnEventRised(ParameterType value)
        {
            response?.Invoke(value);
        }

        private void OnEnable()
        {
            foreach (var gameEvent in gameEvents)
            {
                gameEvent.RegisterListener(this);
            }

        }

        private void OnDisable()
        {
            foreach (var gameEvent in gameEvents)
            {
                gameEvent.UnRegisterListener(this);
            }
        }
    }
}
