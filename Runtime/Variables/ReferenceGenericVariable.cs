
//Class was created based on Ryan Hipple Talk about Game Architecture with Scriptable Objects
//link to talk https://www.youtube.com/watch?v=raQ3iHhE_Kk
//link to github with project https://github.com/roboryantron/Unite2017

using UnityEngine;

[System.Serializable]
public class ReferenceGenericVariable<T, ScriptableObject> where ScriptableObject:IContainValue<T> 
{
    [SerializeField]
    private bool useConstant = true;
    [SerializeField]
    private T constantValue;
    [SerializeField]
    private ScriptableObject variable;

    public T value
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
}
