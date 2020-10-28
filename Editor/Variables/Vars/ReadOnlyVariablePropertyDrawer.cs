using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    /// <summary>
    /// Derived from this class to create custom property drawer for read only variable
    /// </summary>
    public class ReadOnlyVariablePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("variable"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("variable"), label);
        }
    }
}