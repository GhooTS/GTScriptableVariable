using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    [CustomEditor(typeof(StringEventTrigger))]
    public class StringEventTriggerEditor : EventTriggerEditor<StringGameEvent, StringListener, StringEvent, string>
    {

    }
}