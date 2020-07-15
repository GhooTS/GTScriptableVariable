using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    public class GameEventListener : Listener
    {
        public GameEvent[] gameEvents;
        public UnityEvent Response;

        public void OnEventRised()
        {
            Response?.Invoke();
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