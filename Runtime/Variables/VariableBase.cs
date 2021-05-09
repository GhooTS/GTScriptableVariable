

using UnityEngine;

namespace GTVariable
{
    /// <summary>
    /// Base class for all variables
    /// </summary>
    public abstract class VariableBase : ScriptableObject,INameable,IGroupable
    {
        public string group;
        [TextArea]
        [SerializeField]
        public string description;

        public string Name { get { return name; } set { name = value; } }
        public string GroupName { get { return group; } set { group = value; } }
    }
}

