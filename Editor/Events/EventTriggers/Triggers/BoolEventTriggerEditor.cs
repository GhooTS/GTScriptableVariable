using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    [CustomEditor(typeof(BoolEventTrigger))]
    public class BoolEventTriggerEditor : EventTriggerEditor<BoolGameEvent, BoolListener, BoolEvent, bool>
    {

    }
}