

using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    [System.Serializable]
    public class Variable<T> : ScriptableObject
    {
#if UNITY_EDITOR
        [TextArea]
        [SerializeField]
        private string description;
#endif
        public T value;


        public T GetValue()
        {
            return value;
        }

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