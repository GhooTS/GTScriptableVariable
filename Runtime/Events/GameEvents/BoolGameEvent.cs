using UnityEngine;


namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Bool Event")]
    public class BoolGameEvent : GameEvent<Listener<BoolGameEvent,BoolEvent, bool>, BoolEvent, bool>
    {

    }
}

