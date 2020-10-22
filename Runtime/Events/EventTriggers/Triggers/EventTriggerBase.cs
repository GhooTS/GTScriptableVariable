using UnityEngine;

namespace GTVariable
{
    /// <summary>
    /// Base class for all Event Triggers
    /// </summary>
    public abstract class EventTriggerBase : MonoBehaviour
    {
        private void Awake()
        {
            TriggerEvent(UnityEventType.Awake);
        }

        private void OnEnable()
        {
            TriggerEvent(UnityEventType.OnEnable);
        }

        private void Start()
        {
            TriggerEvent(UnityEventType.Start);
        }

        private void OnDisable()
        {
            TriggerEvent(UnityEventType.OnDisable);
        }

        private void OnDestroy()
        {
            TriggerEvent(UnityEventType.OnDestroy);
        }

        protected abstract void TriggerEvent(UnityEventType eventType);
    }
}