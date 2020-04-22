using UnityEngine;


public class TextFloatValueUpdater : TextValueUpdater<FloatVariable, float>
{
    public bool round;

    public override void UpdateValue()
    {
        string str = "";

        if (prefix.Length != 0)
        {
            str = prefix;
        }

        if (round)
        {
            str += Mathf.FloorToInt(value.value);
        }
        else
        {
            str += value.value;
        }
        if (affix.Length != 0)
        {
            str += affix;
        }

        textControl.text = str;
    }
}