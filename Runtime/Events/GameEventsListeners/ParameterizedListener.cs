using UnityEngine;

namespace GTVariable
{
    public abstract class ParameterizedListener<GameEventType,EventType,ParameterType> : Listener,IParameterizedListener<EventType,ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where GameEventType : ParameterizedGameEvent<IParameterizedListener<EventType,ParameterType>,EventType,ParameterType>
    {
        public GameEventType[] gameEvents;
        public EventType response;

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
