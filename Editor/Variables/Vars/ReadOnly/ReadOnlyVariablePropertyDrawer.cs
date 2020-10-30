using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    /// <summary>
    /// Derived from this class to create custom property drawer for <seealso cref="ReadOnlyVariable{VariableType, ParameterType}"/>
    /// </summary>
    public class ReadOnlyVariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative("variable"), label);
        }
    }
}