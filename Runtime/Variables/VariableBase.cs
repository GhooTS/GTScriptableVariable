

using UnityEngine;

namespace GTVariable
{
    public abstract class VariableBase : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        protected string description;
    }
}

