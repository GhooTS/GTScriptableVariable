
// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------


using UnityEditor;
using UnityEngine;




namespace GTVariable.Editor
{
    public class VariableInlineDrawer
    {
        public enum PropertyPosition
        {
            Inline,
            Under
        }

        public PropertyPosition propertyPosition;
        public float inlineWidth = 60f;
        public float inlineSpace = 5f;
        SerializedProperty valueProp;
        SerializedObject serializedObject;

        public VariableInlineDrawer(PropertyPosition propertyPosition = PropertyPosition.Inline)
        {
            this.propertyPosition = propertyPosition;
        }

        public void Update(SerializedProperty property)
        {
            if (property.objectReferenceValue != null)
            {
                serializedObject = new SerializedObject(property.objectReferenceValue);
            }
        }

        public Rect Reserve(ref Rect position)
        {
            if (valueProp == null) return Rect.zero;

            position.height = EditorGUIUtility.singleLineHeight;
            var output = new Rect(position);
            switch (propertyPosition)
            {
                case PropertyPosition.Inline:
                    output.xMin = output.xMax - inlineWidth;
                    position.width -= output.width + inlineSpace;
                    break;
                case PropertyPosition.Under:
                    output.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    output.height = EditorGUI.GetPropertyHeight(valueProp);
                    break;
                default:
                    break;
            }
           
            
            return output;
        }

        public Rect DrawProperty(SerializedProperty property,Rect position)
        {
            if (property.serializedObject != null)
            {
                if (serializedObject == null)
                {
                    serializedObject = new SerializedObject(property.objectReferenceValue);
                }
                valueProp = serializedObject.FindProperty("value");

                serializedObject.Update();

                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, valueProp, GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                serializedObject = null;
                valueProp = null;
            }

            return position;
        }

        public float GetHeight()
        {
            switch (propertyPosition)
            {
                case PropertyPosition.Inline:
                    return 0;
                case PropertyPosition.Under:
                    return valueProp != null ? EditorGUI.GetPropertyHeight(valueProp) + EditorGUIUtility.standardVerticalSpacing : 0;
                default:
                    return 0;
            }
        }
    }
}