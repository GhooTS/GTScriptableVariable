using UnityEngine;
using UnityEngine.UI;

namespace GTVariable
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleWidget : MonoBehaviour
    {
        [SerializeField] private BoolVariable value;
        private Toggle toggle;

        private bool selfTrigger;


        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            toggle.isOn = value;
            value.OnValueChanged.AddListener(OnVariableValueChanaged);
            toggle.onValueChanged.AddListener(OnToggleValueChanaged);
        }

        private void OnDisable()
        {
            value.OnValueChanged.RemoveListener(OnVariableValueChanaged);
            toggle.onValueChanged.RemoveListener(OnToggleValueChanaged);
        }


        private void OnToggleValueChanaged(bool value)
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
            toggle.isOn = value;
        }

    }
}