using UnityEngine;

namespace GTVariable
{
    public class FloatTextWidget : TextWidget<FloatVariable, float>
    {
        public bool rounded;

        protected override string GetValue()
        {
            return rounded ? Mathf.FloorToInt(value.value).ToString() : base.GetValue();
        }
    }
}