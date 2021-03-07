using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    public abstract class GameEventBase : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        private string description;
        /// <summary>
        /// This action is invoke whenever game event is Raise 
        /// </summary>
        public UnityAction OnEventRaised { get; set; }


        public static GameEventBase operator +(GameEventBase gameEvent,UnityAction action)
        {
            gameEvent.OnEventRaised += action;
            return gameEvent;
        }


        public static GameEventBase operator -(GameEventBase gameEvent, UnityAction action)
        {
            gameEvent.OnEventRaised -= action;
            return gameEvent;
        }
    }
}