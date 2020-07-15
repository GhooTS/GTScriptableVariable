

using UnityEditor;
using UnityEngine;




namespace GTVariable.Editor
{


    public class VariablePropertyDrawer : PropertyDrawer
    {
        protected VariableInlineDrawer inlineDrawer = new VariableInlineDrawer();
        private bool initalize = false;
        protected virtual void Init()
        {
            inlineDrawer.inlineWidth = 60f;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + inlineDrawer.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (initalize == false)
            {
                Init();
                initalize = true;
            }

            var hasObjectReference = property.objectReferenceValue != null;
            Rect inlinePosition = Rect.zero;

            EditorGUI.BeginChangeCheck();

            if (hasObjectReference) inlinePosition = inlineDrawer.Reserve(ref position);

            position = EditorGUI.PrefixLabel(position,label);
            EditorGUI.PropertyField(position, property,GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                inlineDrawer.Update(property);
            }

            if(hasObjectReference) inlineDrawer.DrawProperty(property, inlinePosition);
        }
    }
}