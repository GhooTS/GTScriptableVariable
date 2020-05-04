namespace GTVariable
{
    public class TextBoolValueUpdater : TextValueUpdater<BoolVariable, bool>
    {
        public string trueValue;
        public string falseValue;

        public override void UpdateValue()
        {
            string str = "";

            if (prefix.Length != 0)
            {
                str = prefix;
            }

            str += value.value ? trueValue : falseValue;

            if (affix.Length != 0)
            {
                str += affix;
            }

            textControl.text = str;
        }
    }
}