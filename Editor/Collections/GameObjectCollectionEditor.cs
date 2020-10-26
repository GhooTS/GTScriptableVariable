using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(GameObjectCollection))]
    public class GameObjectCollectionEditor : UnityEditor.Editor
    {
        private GameObjectCollection collection;

        private void OnEnable()
        {
            collection = target as GameObjectCollection;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.LabelField("Number of elements", collection.Count.ToString());
            EditorGUI.BeginDisabledGroup(true);
            for (int i = 0; i < collection.Count; i++)
            {
                EditorGUILayout.ObjectField(collection[i],typeof(GameObject),false);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}