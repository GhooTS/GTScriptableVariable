using UnityEngine;
using UnityEditor;



namespace GTVariable.Editor
{
    [CustomEditor(typeof(IntCollisionEventTrigger))]
    public class IntCollisionEventTriggerEditor : CollisionEventTriggerEditor<IntGameEvent, IntEvent, int>
    {

    }
}