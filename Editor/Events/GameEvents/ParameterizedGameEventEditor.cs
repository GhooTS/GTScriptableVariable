using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{


    public class ParameterizedGameEventEditor<ListenerType, GameEventType, EventType, ParameterType> : GameEventEditorBase<GameEventType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where ListenerType : ParameterizedListener<GameEventType, EventType, ParameterType>
        where GameEventType : ParameterizedGameEvent<ParameterizedListener<GameEventType, EventType, ParameterType>, EventType, ParameterType>
    {
        protected ParameterType parameter = default;

        public override void RaiseEvent()
        {
            gameEvent.Raise(parameter);
        }
    }
}
