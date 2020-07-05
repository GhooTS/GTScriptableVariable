using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/EventVars/Color")]
    public class ColorEventVariable : ColorVariable
    {
        public GameEvent valueChaged;

        public override void SetValue(Color value)
        {
            base.SetValue(value);
            if (valueChaged != null)
            {
                valueChaged.Raise();
            }
        }
    }
}