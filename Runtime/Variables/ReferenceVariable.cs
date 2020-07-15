
//Class was created based on Ryan Hipple Talk about Game Architecture with Scriptable Objects
//link to talk https://www.youtube.com/watch?v=raQ3iHhE_Kk
//link to github with project https://github.com/roboryantron/Unite2017

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