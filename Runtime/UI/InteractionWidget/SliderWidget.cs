using UnityEngine;
using UnityEngine.UI;

namespace GTVariable
{

    [RequireComponent(typeof(Slider))]
    public class SliderWidget : MonoBehaviour
    {
        [SerializeField] private FloatVariable value;
        private Slider slider;

        private bool selfTrigger;


        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            slider.value = value;
            value.OnValueChanged.AddListener(OnVariableValueChanaged);
            slider.onValueChanged.AddListener(OnSliderValueChanaged);
        }

        private void OnDisable()
        {
            value.OnValueChanged.RemoveListener(OnVariableValueChanaged);
            slider.onValueChanged.RemoveListener(OnSliderValueChanaged);
        }

        private void OnSliderValueChanaged(float value)
        {
            if (selfTrigger)
            {
                selfTrigger = false;
                return;
            }

            selfTrigger = true;
            this.value.SetValue(value);
        }

        private void OnVariableValueChanaged()
        {
            if (selfTrigger)
            {
                selfTrigger = false;
                return;
            }

            selfTrigger = true;
            slider.value = value;
        }

    }
}