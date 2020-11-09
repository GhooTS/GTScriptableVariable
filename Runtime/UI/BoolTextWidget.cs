using UnityEngine;

namespace GTVariable
{

    public class BoolTextWidget : TextWidget<ReadOnlyBoolVariable,BoolVariable, bool>
    {
        [Tooltip("Text that appear if value was true")]
        public StringReference trueValue;
        [Tooltip("Text that appear if value was false")]
        public StringReference falseValue;

        protected override string GetValue()
        {
            return value.GetValue() ? trueValue : falseValue;
        }
    }
}