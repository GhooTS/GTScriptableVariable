

using UnityEngine;

namespace GTVariable
{
    [System.Serializable]
    public class ReferenceVariable<T, VariableType> where VariableType : Variable<T>
    {
        [SerializeField]
        private bool useConstant = true;
        [SerializeField]
        private T constantValue;
        [SerializeField]
        private VariableType variable;

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
                    variable.SetValue(value);
                }
            }
        }

        public static implicit operator T(ReferenceVariable<T, VariableType> genericVariable)
        {
            return genericVariable.Value;
        }
    }
}