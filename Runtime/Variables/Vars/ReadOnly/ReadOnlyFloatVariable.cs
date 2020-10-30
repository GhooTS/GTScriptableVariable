using UnityEngine;
using UnityEditor;

namespace GTVariable
{
    [System.Serializable]
    public class ReadOnlyFloatVariable : ReadOnlyVariable<FloatVariable, float>
    {
        public ReadOnlyFloatVariable(FloatVariable variable) : base(variable)
        {

        }
    }
}