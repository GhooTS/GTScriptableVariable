using UnityEngine;

namespace GTVariable
{
    public class FloatTextWidget : TextWidget<ReadOnlyFloatVariable,FloatVariable, float>
    {
        public bool rounded;

        protected override string GetValue()
        {
            return rounded ? Mathf.FloorToInt(value.GetValue()).ToString() : base.GetValue();
        }
    }
}