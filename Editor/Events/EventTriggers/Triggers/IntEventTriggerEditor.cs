using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    [CustomEditor(typeof(IntEventTrigger))]
    public class IntEventTriggerEditor : EventTriggerEditor<IntGameEvent, IntListener, IntEvent, int>
    {

    }
}