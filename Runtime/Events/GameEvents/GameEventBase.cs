using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{
    public abstract class GameEventBase : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        private string description;

        public abstract void RegisterListener(Listener listner);
        public abstract void UnRegisterListener(Listener listner);
    }
}