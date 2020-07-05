using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/EventVars/Int")]
    public class IntEventVariable : IntVariable
    {
        public GameEvent valueChaged;

        public override void SetValue(int value)
        {
            base.SetValue(value);
            if (valueChaged != null)
            {
                valueChaged.Raise();
            }
        }
    }
}