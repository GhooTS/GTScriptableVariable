using UnityEngine;

namespace GTVariable
{

    public class DynamicSubscriber : MonoBehaviour
    {
        public DynamicCollectionBase[] collections;

        private void OnEnable()
        {
            if (collections == null) return;

            foreach (var collection in collections)
            {
                if (collection != null) collection.Subscribe(gameObject);
            }
        }

        private void OnDisable()
        {
            if (collections == null) return;

            foreach (var collection in collections)
            {
                if(collection != null) collection.Unsubscribe(gameObject);
            }
        }
    }
}