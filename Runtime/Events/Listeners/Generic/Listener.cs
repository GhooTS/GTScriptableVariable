﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GTVariable
{

    /// <summary>
    /// Derive from this class to create custom one argument listener
    /// </summary>
    public abstract class Listener<GameEventType, EventType, ParameterType> : Listener, IListener<EventType, ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where GameEventType : GameEvent<Listener<GameEventType, EventType, ParameterType>, EventType, ParameterType>
    {

        /// <summary>
        /// List of game events to which this listener subscribe to
        /// </summary>
        public List<GameEventType> gameEvents;

        /// <summary>
        /// Response which will be call <seealso cref="OnEventRised(ParameterType)"/>
        /// </summary>
        public EventType response;

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

        /// <summary>
        /// Invoke <seealso cref="response"/> with specify value
        /// </summary>
        public void OnEventRised(ParameterType value)
        {
            response?.Invoke(value);
        }

        public override void OnEventRised()
        {
            response?.Invoke(default);
        }

        public override UnityEventBase GetResponse()
        {
            return response;
        }

        public override List<GameEventBase> GetGameEvents()
        {
            return gameEvents as List<GameEventBase>;
        }
    }
}
