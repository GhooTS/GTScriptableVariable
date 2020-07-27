

using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Float Event")]
    public class FloatGameEvent : ParameterizedGameEvent<IParameterizedListener<FloatEvent, float>, FloatEvent, float>
    {

    }
}