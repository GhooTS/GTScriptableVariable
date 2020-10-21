using UnityEngine;


namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Bool Event")]
    public class BoolGameEvent : ParameterizedGameEvent<ParameterizedListener<BoolGameEvent,BoolEvent, bool>, BoolEvent, bool>
    {

    }
}

