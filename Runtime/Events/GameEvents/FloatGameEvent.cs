

using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Float Event")]
    public class FloatGameEvent : ParameterizedGameEvent<ParameterizedListener<FloatGameEvent,FloatEvent, float>, FloatEvent, float>
    {

    }
}