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
        [Tooltip("If tick, text control will be updated whenever variable OnValueChanged event is raised")]
        [SerializeField]
        private bool autoUpdate;

        protected virtual void OnEnable()
        {
            if (updateOnEnable)
            {
                UpdateValue();
            }
            if (autoUpdate)
            {
                value?.OnValueChanaged.AddListener(UpdateValue);
            }
        }

        private void OnDisable()
        {
            if (autoUpdate)
            {
                value?.OnValueChanaged.RemoveListener(UpdateValue);
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