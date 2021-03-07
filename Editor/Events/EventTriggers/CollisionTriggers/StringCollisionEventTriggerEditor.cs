using UnityEngine;
using UnityEditor;



namespace GTVariable.Editor
{
    [CustomEditor(typeof(StringCollisionEventTrigger))]
    public class StringCollisionEventTriggerEditor : CollisionEventTriggerEditor<StringGameEvent, StringEvent, string>
    {

    }
}