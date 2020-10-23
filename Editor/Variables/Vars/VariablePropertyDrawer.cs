using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    /// <summary>
    /// Derive from this class to create custom variable property drawer
    /// </summary>
    public class VariablePropertyDrawer : PropertyDrawer
    {
        protected VariableInlineDrawer inlineDrawer = new VariableInlineDrawer();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + inlineDrawer.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            inlineDrawer.Update(property);
            inlineDrawer.DrawWrapper(ref position);
            Rect inlinePosition = inlineDrawer.Reserve(ref position);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property);
            inlineDrawer.DrawProperty(property,inlinePosition);
        }
    }
}