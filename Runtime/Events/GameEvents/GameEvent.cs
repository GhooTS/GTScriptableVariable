

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{

    /// <summary>
    /// Zero argument game event
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Event")]
    public class GameEvent : GameEventBase
    {
        public void Raise()
        {
            OnEventRaised?.Invoke();
        }

        public static GameEvent operator +(GameEvent gameEvent, UnityAction action)
        {
            gameEvent.OnEventRaised += action;
            return gameEvent;
        }

        public static GameEvent operator -(GameEvent gameEvent, UnityAction action)
        {
            gameEvent.OnEventRaised -= action;
            return gameEvent;
        }

    }
}