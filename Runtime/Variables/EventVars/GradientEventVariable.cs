using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/EventVars/Gradient")]
    public class GradientEventVariable : GradientVariable
    {
        public GameEvent valueChaged;

        public override void SetValue(UnityEngine.Gradient value)
        {
            base.SetValue(value);
            if (valueChaged != null)
            {
                valueChaged.Raise();
            }
        }
    }
}