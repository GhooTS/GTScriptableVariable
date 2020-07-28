using UnityEngine;

namespace GTVariable
{
    public abstract class Listener : MonoBehaviour
    {
#if UNITY_EDITOR
        public string listenerName;
        [TextArea]
        [SerializeField]
        private string listenerDescription;
#endif

    }
}