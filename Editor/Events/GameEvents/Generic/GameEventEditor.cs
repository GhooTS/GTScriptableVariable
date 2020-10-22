using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    /// <summary>
    /// Derive from this class to create custom one argument game event editor
    /// </summary>
    public class GameEventEditor<ListenerType, GameEventType, EventType, ParameterType> : GameEventEditorBase<GameEventType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where ListenerType : Listener<GameEventType, EventType, ParameterType>
        where GameEventType : GameEvent<Listener<GameEventType, EventType, ParameterType>, EventType, ParameterType>
    {
        protected ParameterType parameter = default;

        public override void RaiseEvent()
        {
            gameEvent.Raise(parameter);
        }
    }
}
