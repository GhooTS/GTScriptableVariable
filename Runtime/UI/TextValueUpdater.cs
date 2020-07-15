using TMPro;
using UnityEngine;

namespace GTVariable
{
    public class TextValueUpdater<T, N> : MonoBehaviour
        where T : Variable<N>
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

        public void UpdateValue()
        {
            string str = "";

            if (prefix.Length != 0)
            {
                str = prefix;
            }

            str += GetValue();

            if (affix.Length != 0)
            {
                str += affix;
            }

            textControl.text = str;
        }

        protected virtual string GetValue()
        {
            return $"{value.value}";
        }

        protected void OnEnableBase()
        {
            if (updateOnEnable)
            {
                UpdateValue();
            }
        }
    }
}