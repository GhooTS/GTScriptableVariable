using UnityEngine;

namespace GTVariable
{
    public abstract class GameEventBase : ScriptableObject
    {
#if UNITY_EDITOR
        [TextArea]
        [SerializeField]
        private string description;
#endif
    }
}