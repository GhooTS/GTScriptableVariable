using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(IntGameEvent))]
    public class IntGameEventEditor 
        : ParameterizedGameEventEditor<IntListener, IntGameEvent, IntEvent, int>
    {

    }
}