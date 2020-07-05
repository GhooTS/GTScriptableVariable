using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/EventVars/String")]
    public class StringEventVariable : StringVariable
    {
        public GameEvent valueChaged;

        public override void SetValue(string value)
        {
            base.SetValue(value);
            if (valueChaged != null)
            {
                valueChaged.Raise();
            }
        }
    }
}