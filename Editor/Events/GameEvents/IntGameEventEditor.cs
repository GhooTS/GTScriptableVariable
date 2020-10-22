using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(IntGameEvent))]
    public class IntGameEventEditor 
        : GameEventEditor<IntListener, IntGameEvent, IntEvent, int>
    {

    }
}