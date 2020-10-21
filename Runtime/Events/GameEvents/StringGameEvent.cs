using UnityEngine;


namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/String Event")]
    public class StringGameEvent : ParameterizedGameEvent<ParameterizedListener<StringGameEvent,StringEvent, string>, StringEvent, string>
    {

    }
}

