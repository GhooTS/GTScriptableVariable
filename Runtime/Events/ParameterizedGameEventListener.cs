using UnityEngine;

namespace GTVariable
{
    public abstract class ParameterizedGameEventListener<GameEventType,EventType,ParameterType> : Listener,IRaisable<EventType,ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where GameEventType : ParameterizedGameEvent<IRaisable<EventType,ParameterType>,EventType,ParameterType>
    {
        public GameEventType[] gameEvents;
        public EventType Response;

        public void OnEventRised(ParameterType value)
        {
            Response?.Invoke(value);
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
