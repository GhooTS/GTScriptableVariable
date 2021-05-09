using TMPro;
using UnityEngine;

namespace GTVariable
{
    public class TextWidget<ReadOnlyVariableType,VariableType, ParameterType> : MonoBehaviour
        where VariableType : Variable<ParameterType>
    {
        [Header("Display options")]
        [Tooltip("Text that will appear before variable value")]
        public string prefix;
        [Tooltip("Text that will appear after variable value")]
        public string affix;
        public VariableType value;
        public TextMeshProUGUI textControl;
        [Header("Update options")]
        [Tooltip("If tick, text control will be update on enable")]
        public bool updateOnEnable;
        [Tooltip("In which mode widget should operated")]
        [SerializeField]
        private UpdateMode updateMode = UpdateMode.Event;
        [Header("Variable Loading Options")]
        public string variableName;
        public bool createVariable;

        protected virtual void OnEnable()
        {
            if(value == null) WidgetVariableHelper.CreateVariableIfNeeded(value,variableName,createVariable);

            if (updateOnEnable)
            {
                UpdateValue();
            }
            if (updateMode == UpdateMode.Event)
            {
                value?.OnValueChanged.AddListener(UpdateValue);
            }
        }

        private void OnDisable()
        {
            if (updateMode == UpdateMode.Event)
            {
                value?.OnValueChanged.RemoveListener(UpdateValue);
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
            if (value == null) return;

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