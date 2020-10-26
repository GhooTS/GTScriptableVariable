using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "ScriptableVars/Collections/GameObject Collection")]
    public class GameObjectCollection : DynamicCollectionBase
    {
        protected readonly List<GameObject> elements = new List<GameObject>();
        public ReadOnlyCollection<GameObject> Elements { get { return elements.AsReadOnly(); } }

        public override int Count { get { return elements.Count; } }

        public GameObject this[int index]
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
            if (!Contain(gameObject))
            {
                elements.Add(gameObject);
                return true;
            }

            return false;
        }
        public override bool Unsubscribe(GameObject gameObject)
        {

            return elements.Remove(gameObject);

        }


        public bool Contain(GameObject element)
        {
            return elements.Contains(element);
        }
    }
}