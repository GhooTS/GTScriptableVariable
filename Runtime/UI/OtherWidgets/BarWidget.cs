using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GTVariable
{

    public class BarWidget : MonoBehaviour
    {
        [SerializeField] private ReadOnlyFloatVariable currentValue;
        [SerializeField] private ReadOnlyFloatVariable maxValue;
        [SerializeField] private Image fillBar;
        [SerializeField] private UpdateMode updateMode = UpdateMode.Event;
        [SerializeField] private bool UpdateOnEnable;

        private void OnEnable()
        {
            if (UpdateOnEnable)
            {
                UpdateWidget();
            }

            if(updateMode == UpdateMode.Event)
            {
                currentValue.OnValueChanaged.AddListener(UpdateWidget);
                maxValue.OnValueChanaged.AddListener(UpdateWidget);
            }
        }

        private void OnDisable()
        {
            if (updateMode == UpdateMode.Event)
            {
                currentValue.OnValueChanaged.RemoveListener(UpdateWidget);
                maxValue.OnValueChanaged.RemoveListener(UpdateWidget);
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
            fillBar.fillAmount = currentValue.GetValue() / maxValue.GetValue();
        }
    }
}