using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GTVariable
{
    /// <summary>
    /// Base class for all listener
    /// </summary>
    public abstract class Listener : MonoBehaviour
    {
        public string listenerName;
        [TextArea]
        [SerializeField]
        public string listenerDescription;

        public abstract void OnEventRised();
        public abstract UnityEventBase GetResponse();
        public abstract List<GameEventBase> GetGameEvents();


    }
}