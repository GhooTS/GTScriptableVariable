using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GTVariable
{

    public class BarWidget : MonoBehaviour
    {
        [SerializeField] private FloatVariable currentValue;
        [SerializeField] private FloatVariable maxValue;
        [SerializeField] private Image fillBar;
        [SerializeField] private UpdateMode updateMode = UpdateMode.Event;
        [SerializeField] private bool UpdateOnEnable;
        [Header("Variable Loading Options")]
        [SerializeField] private bool createCurrentValue;
        [SerializeField] private string currentValueName;
        [SerializeField] private bool createMaxValue;
        [SerializeField] private string maxValueName;

        private void OnEnable()
        {
            if (currentValue == null) WidgetVariableHelper.CreateVariableIfNeeded(currentValue, currentValueName, createCurrentValue);
            if (currentValue == null) WidgetVariableHelper.CreateVariableIfNeeded(maxValue, maxValueName, createMaxValue);

            if (UpdateOnEnable)
            {
                UpdateWidget();
            }

            if(updateMode == UpdateMode.Event)
            {
                if (currentValue != null) currentValue.OnValueChanged.AddListener(UpdateWidget);
                if (maxValue != null) maxValue.OnValueChanged.AddListener(UpdateWidget);
            }
        }

        private void OnDisable()
        {
            if (updateMode == UpdateMode.Event)
            {
                if (currentValue != null) currentValue.OnValueChanged.RemoveListener(UpdateWidget);
                if (maxValue != null) maxValue.OnValueChanged.RemoveListener(UpdateWidget);
            }
        }

        private void Update()
        {
            if(updateMode == UpdateMode.Update)
            {
                UpdateWidget();
            }
        }



        public void UpdateWidget()
        {
            if (currentValue != null || maxValue == null) return;

            fillBar.fillAmount = currentValue.GetValue() / maxValue.GetValue();
        }
    }
}