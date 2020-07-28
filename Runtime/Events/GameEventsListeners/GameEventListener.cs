using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    public class GameEventListener : Listener
    {
        public GameEvent[] gameEvents;
        public UnityEvent response;

        public void OnEventRised()
        {
            response?.Invoke();
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