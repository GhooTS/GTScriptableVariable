using UnityEngine;
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
        public T defaultValue;
        public bool resetOnEnabled;

        [SerializeField]
        private UnityEvent onValueChanged = new UnityEvent();

        public UnityEvent OnValueChanged => onValueChanged;


        protected virtual void OnEnable()
        {
            if (resetOnEnabled)
            {
                value = defaultValue;
            }
        }

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

            onValueChanged?.Invoke();
        }

        public static implicit operator T(Variable<T> genericVariable)
        {
            return genericVariable.value;
        }
    }
}

