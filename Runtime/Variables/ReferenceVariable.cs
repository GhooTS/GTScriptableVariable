

using UnityEngine;

namespace GTVariable
{
    /// <summary>
    /// Derive from this class to create custom reference variable
    /// </summary>
    [System.Serializable]
    public class ReferenceVariable<T, VariableType> where VariableType : Variable<T>
    {

        /// <summary>
        /// use constant value or assign variable? 
        /// </summary>
        [SerializeField]
        public bool useConstant = true;
        [SerializeField]
        private T constantValue;
        [SerializeField]
        private VariableType variable;

        /// <summary>
        /// Value of this reference variable
        /// </summary>
        /// <remarks>You will modify/access constant value or variable depends on the <seealso cref="UseConstant"/></remarks>
        public T Value
        {
            get
            {
                return useConstant ? constantValue : variable.GetValue();
            }
            set
            {
                if (useConstant)
                {
                    constantValue = value;
                }
                else
                {
                    variable.value = value;
                }
            }
        }

        /// <summary>
        /// Use this method over <seealso cref="Value"/> property to raise <seealso cref="Variable{T}.onValueChanged"/> event for variable
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(T value)
        {
            if (useConstant)
            {
                constantValue = value;
            }
            else
            {
                variable.SetValue(value);
            }
        }

        public static implicit operator T(ReferenceVariable<T, VariableType> genericVariable)
        {
            return genericVariable.Value;
        }
    }
}