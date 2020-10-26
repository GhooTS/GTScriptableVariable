using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor
{
    /// <summary>
    /// Derive from this class to create your custom dynamic collection editor
    /// </summary>
    public class DynamicCollectionEditor<CollectionType,T> : UnityEditor.Editor
        where CollectionType : DynamicCollection<T>
        where T : Component
    {
        private CollectionType collection;

        private void OnEnable()
        {
            collection = target as CollectionType;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.LabelField("Number of elements", collection.Count.ToString());
            EditorGUI.BeginDisabledGroup(true);
            for (int i = 0; i < collection.Count; i++)
            {
                EditorGUILayout.ObjectField(collection[i], typeof(GameObject), false);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}