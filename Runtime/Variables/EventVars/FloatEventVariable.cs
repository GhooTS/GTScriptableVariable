using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/EventVars/Float")]
    public class FloatEventVariable : FloatVariable
    {
        public GameEvent valueChaged;

        public override void SetValue(float value)
        {
            base.SetValue(value);
            if (valueChaged != null)
            {
                valueChaged.Raise();
            }
        }
    }
}