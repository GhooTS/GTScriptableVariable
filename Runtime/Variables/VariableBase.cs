

using UnityEngine;

namespace GTVariable
{
    /// <summary>
    /// Base class for all variables
    /// </summary>
    public abstract class VariableBase : ScriptableObject
    {
        [TextArea]
        [SerializeField]
        protected string description;
    }
}

