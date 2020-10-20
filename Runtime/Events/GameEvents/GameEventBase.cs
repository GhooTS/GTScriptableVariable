using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{
    public abstract class GameEventBase : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        private string description;
        public abstract List<Listener> Listeners { get; }
    }
}