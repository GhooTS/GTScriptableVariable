using UnityEngine;

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