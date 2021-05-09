

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
        public UnityEvent OnValueChanged => variable.OnValueChanged;

        public ReadOnlyVariable(VariableType variable)
        {
            this.variable = variable;
        }

        public void SetVariable(VariableType variable)
        {
            this.variable = variable;
        }

        public ParameterType GetValue()
        {
            return variable == null ? default : variable.GetValue();
        }

        public bool HasVariable()
        {
            return variable != null;
        }
    }
}

