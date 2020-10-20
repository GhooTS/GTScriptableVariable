using UnityEngine;
using System.Collections.Generic;

namespace GTVariable
{
    public abstract class Listener : MonoBehaviour
    {
        public string listenerName;
        [TextArea]
        [SerializeField]
        private string listenerDescription;
    }
}