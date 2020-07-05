using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/EventVars/Bool")]
    public class BoolEventVariable : BoolVariable
    {
        public GameEvent valueChaged;

        public override void SetValue(bool value)
        {
            base.SetValue(value);
            if (valueChaged != null)
            {
                valueChaged.Raise();
            }
        }
    }
}