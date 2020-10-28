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
            return EditorGUI.GetPropertyHeight(property, label) + inlineDrawer.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            inlineDrawer.Update(property);
            inlineDrawer.DrawWrapper(ref position);
            //Cache label text, because for some reasone EditorGUI.GetPropertyHeight 
            //chanage label text...
            var labelText = label.text;
            Rect inlinePosition = inlineDrawer.Reserve(ref position);
            label.text = labelText;
            EditorGUI.ObjectField(position, property, label);
            inlineDrawer.DrawProperty(property,inlinePosition);
        }
    }
}