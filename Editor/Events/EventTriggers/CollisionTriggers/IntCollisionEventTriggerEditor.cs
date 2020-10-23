using UnityEngine;
using UnityEditor;



namespace GTVariable.Editor
{
    [CustomEditor(typeof(IntCollisionEventTrigger))]
    public class IntCollisionEventTriggerEditor : CollisionEventTriggerEditor<IntGameEvent, IntListener, IntEvent, int>
    {

    }
}