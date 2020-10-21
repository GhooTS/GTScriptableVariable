using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GTVariable
{
    public class GameEventListener : Listener
    {
        /// <summary>
        /// List of game events to which this listener subscribe to
        /// </summary>
        public List<GameEventBase> gameEvents;

        /// <summary>
        /// Response which will be call <seealso cref="OnEventRised()"/>
        /// </summary>
        public UnityEvent response;

        /// <summary>
        /// Invoke <seealso cref="response"/> with specify value
        /// </summary>
        public override void OnEventRised()
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

        public override UnityEventBase GetResponse()
        {
            return response;
        }

        public override List<GameEventBase> GetGameEvents()
        {
            return gameEvents;
        }
    }
}