using UnityEditor;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(FloatCollisionEventTrigger))]
    public class FloatCollisionEventTriggerEditor : CollisionEventTriggerEditor<FloatGameEvent, FloatListener, FloatEvent, float>
    {

    }
}