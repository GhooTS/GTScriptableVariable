using UnityEngine;

namespace GTVariable
{

    /// <summary>
    /// Base class for all Collsion Event Triggers
    /// </summary>
    public abstract class CollisionEventTriggerBase : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            TriggerEvent(PhysicEventType.OnCollisionEnter,collision.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TriggerEvent(PhysicEventType.OnCollisionEnter2D, collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            TriggerEvent(PhysicEventType.OnCollisionExit, collision.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            TriggerEvent(PhysicEventType.OnCollisionExit2D, collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEvent(PhysicEventType.OnTriggerEnter, other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TriggerEvent(PhysicEventType.OnTriggerEnter2D, collision.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerEvent(PhysicEventType.OnTriggerExit, other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            TriggerEvent(PhysicEventType.OnTriggerExit2D, collision.gameObject);
        }

        protected abstract void TriggerEvent(PhysicEventType eventType,GameObject gameObject);
    }
}