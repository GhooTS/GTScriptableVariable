using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;
using UnityEngine;

namespace GTVariable
{
    public class DynamicCollection<T> : DynamicCollectionBase
        where T : Component
    {
        protected readonly List<T> elements = new List<T>();
        public ReadOnlyCollection<T> Elements { get { return elements.AsReadOnly(); } }

        public override int Count { get { return elements.Count; } }

        public T this[int index]
        {
            get { return elements[index]; }
        }

        private void OnDisable()
        {
            //Collection should always be empty, when out of scoupe
            Assert.IsTrue(Count == 0);
            elements.Clear();
        }

        public override bool Subscribe(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out T subscriber) && !Contain(subscriber))
            {
                elements.Add(subscriber);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Use this method to subscribe to the collection
        /// </summary>
        /// <param name="subscriber">Subscriber of this collection</param>
        /// <returns>returns whether subscription was successful</returns>
        public bool Subscribe(T subscriber)
        {
            if (subscriber != null && !Contain(subscriber))
            {
                elements.Add(subscriber);
                return true;
            }

            return false;
        }

        public override bool Unsubscribe(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out T subscriber))
            {
                return elements.Remove(subscriber);
            }

            return false;
        }

        /// <summary>
        /// Use this method to unsubscribe from the collection
        /// </summary>
        /// <param name="subscriber">Subscriber of this collectionr</param>
        /// <returns>returns whether unsubscription was successful</returns>
        public bool Unsubscribe(T subscriber)
        {
            return elements.Remove(subscriber);
        }

        public bool Contain(T element)
        {
            return elements.Contains(element);
        }

    }
}