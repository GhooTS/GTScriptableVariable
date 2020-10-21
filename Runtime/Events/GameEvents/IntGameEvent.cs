using UnityEngine;


namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Int Event")]
    public class IntGameEvent : ParameterizedGameEvent<ParameterizedListener<IntGameEvent,IntEvent, int>, IntEvent, int>
    {

    }
}

