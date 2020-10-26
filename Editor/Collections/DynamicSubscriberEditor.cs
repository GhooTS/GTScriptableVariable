using UnityEditor;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(DynamicSubscriber))]
    public partial class DynamicSubscriberEditor : UnityEditor.Editor
    {
        private SerializedProperty collections;

        private void OnEnable()
        {
            collections = serializedObject.FindProperty("collections");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GTGUILayout.ArrayProperty(collections,"Collections");
            serializedObject.ApplyModifiedProperties();
        }
    }
}