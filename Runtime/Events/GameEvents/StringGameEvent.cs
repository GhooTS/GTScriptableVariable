using UnityEngine;


namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/String Event")]
    public class StringGameEvent : GameEvent<Listener<StringGameEvent,StringEvent, string>, StringEvent, string>
    {

    }
}

