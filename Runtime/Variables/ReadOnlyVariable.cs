﻿

using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    [System.Serializable]
    public class ReadOnlyVariable<VariableType,ParameterType> 
        where VariableType : Variable<ParameterType>
    {
        [SerializeField]
        private VariableType variable;
        public UnityEvent OnValueChanaged => variable.OnValueChanaged;

        public ReadOnlyVariable(VariableType variable)
        {
            this.variable = variable;
        }

        public ParameterType GetValue()
        {
            return variable.GetValue();
        }
    }
}

