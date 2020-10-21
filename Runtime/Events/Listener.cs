using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GTVariable
{
    public abstract class Listener : MonoBehaviour
    {
        public string listenerName;
        [TextArea]
        [SerializeField]
        private string listenerDescription;

        public abstract void OnEventRised();
        public abstract UnityEventBase GetResponse();
        public abstract List<GameEventBase> GetGameEvents();


    }
}