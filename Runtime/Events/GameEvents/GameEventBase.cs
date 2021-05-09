using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    public abstract class GameEventBase : ScriptableObject,INameable,IGroupable
    {
        public string group;
        [TextArea]
        [SerializeField]
        public string description;
        /// <summary>
        /// This action is invoke whenever game event is Raise 
        /// </summary>
        public UnityAction OnEventRaised { get; set; }
        public string Name { get { return name; } set { name = value; } }
        public string GroupName { get { return group; } set { group = value; } }

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