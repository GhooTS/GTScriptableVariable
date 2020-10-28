using UnityEngine.Events;

namespace GTVariable
{

    /// <summary>
    /// Derive from this class to create custom variable
    /// </summary>
    [System.Serializable]
    public class Variable<T> : VariableBase
    {
        /// <summary>
        /// Value of this variable
        /// </summary>
        public T value;


        /// <summary>
        /// Get value of this variable
        /// </summary>
        public T GetValue()
        {
            return value;
        }

        /// <summary>
        /// Set value of this variable
        /// </summary>
        /// <remarks>This method will also call an event for event variable. If this is not intended use <seealso cref="value"/> field instead</remarks>
        public virtual void SetValue(T value)
        {
            this.value = value;
        }

        public static implicit operator T(Variable<T> genericVariable)
        {
            return genericVariable.value;
        }
    }
}

