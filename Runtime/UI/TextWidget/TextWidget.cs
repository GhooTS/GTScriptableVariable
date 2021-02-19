using TMPro;
using UnityEngine;

namespace GTVariable
{
    public class TextWidget<ReadOnlyVariableType,VariableType, ParameterType> : MonoBehaviour
        where ReadOnlyVariableType : ReadOnlyVariable<VariableType,ParameterType>
        where VariableType : Variable<ParameterType>
    {
        [Tooltip("Text that will appear before variable value")]
        public string prefix;
        [Tooltip("Text that will appear after variable value")]
        public string affix;
        public ReadOnlyVariableType value;
        public TextMeshProUGUI textControl;
        [Tooltip("If tick, text control will be update on enable")]
        public bool updateOnEnable;
        [Tooltip("In which mode widget should operated")]
        [SerializeField]
        private UpdateMode updateMode = UpdateMode.Event;

        protected virtual void OnEnable()
        {
            if (updateOnEnable)
            {
                UpdateValue();
            }
            if (updateMode == UpdateMode.Event)
            {
                value?.OnValueChanaged.AddListener(UpdateValue);
            }
        }

        private void OnDisable()
        {
            if (updateMode == UpdateMode.Event)
            {
                value?.OnValueChanaged.RemoveListener(UpdateValue);
            }
        }

        private void Update()
        {
            if (updateMode == UpdateMode.Update)
            {
                UpdateValue();
            }
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
            return value.GetValue().ToString();
        }
    }
}