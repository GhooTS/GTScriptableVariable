using UnityEngine;
using TMPro;
using System.Text;

public class TextValueUpdater<T,N> : MonoBehaviour 
    where T : GenericVariable<N>
{
    public string prefix;
    public string affix;
    public T value;
    public TextMeshProUGUI textControl;
    public bool updateOnEnable;

    private void OnEnable()
    {
        OnEnableBase();
    }

    public virtual void UpdateValue()
    {
        string str = "";

        if (prefix.Length != 0)
        {
            str = prefix;
        }

        str += $"{value.value}";

        if(affix.Length != 0)
        {
            str += affix;
        }

        textControl.text = str;
    }

    protected void OnEnableBase()
    {
        if (updateOnEnable)
        {
            UpdateValue();
        }
    }
}
