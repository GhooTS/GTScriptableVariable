namespace GTVariable
{
    public class TextBoolValueUpdater : TextValueUpdater<BoolVariable, bool>
    {
        public StringReference trueValue;
        public StringReference falseValue;

        protected override string GetValue()
        {
            return value.value ? trueValue : falseValue;
        }
    }
}