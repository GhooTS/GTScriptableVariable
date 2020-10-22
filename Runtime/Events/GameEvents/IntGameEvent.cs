using UnityEngine;


namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Int Event")]
    public class IntGameEvent : GameEvent<Listener<IntGameEvent,IntEvent, int>, IntEvent, int>
    {

    }
}

