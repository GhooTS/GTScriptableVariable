
//Class was created based on Ryan Hipple Talk about Game Architecture with Scriptable Objects
//link to talk https://www.youtube.com/watch?v=raQ3iHhE_Kk
//link to github with project https://github.com/roboryantron/Unite2017

using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{
    [System.Serializable]
    public class Variable<T> : ScriptableObject
    {
        public T value;
        public UnityEvent onValueChange;


        public T GetValue()
        {
            return value;
        }

        public void SetValue(T value)
        {
            this.value = value;
        }

        public void SetValueWithEvent(T value)
        {
            this.value = value;
            onValueChange?.Invoke();
        }

        public static implicit operator T(Variable<T> genericVariable)
        {
            return genericVariable.value;
        }
    }
}