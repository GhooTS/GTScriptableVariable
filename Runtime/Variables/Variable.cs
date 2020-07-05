
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