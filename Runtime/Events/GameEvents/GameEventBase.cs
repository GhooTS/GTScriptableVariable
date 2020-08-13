using UnityEngine;

namespace GTVariable
{
    public abstract class GameEventBase : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        private string description;
    }
}