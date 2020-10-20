using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    public class GameEventListener : Listener
    {
        /// <summary>
        /// List of game events to which this listener subscribe to
        /// </summary>
        public GameEvent[] gameEvents;

        /// <summary>
        /// Response which will be call <seealso cref="OnEventRised()"/>
        /// </summary>
        public UnityEvent response;

        /// <summary>
        /// Invoke <seealso cref="response"/> with specify value
        /// </summary>
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