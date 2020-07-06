
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

    public class VariablePropertyDrawer : PropertyDrawer
    {
        SerializedProperty valueProp;
        SerializedObject serializedObject;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyHeight = EditorGUI.GetPropertyHeight(property);
            
            EditorGUI.BeginChangeCheck();
            position = EditorGUI.PrefixLabel(position,label);
            var valuePosition = new Rect(position.xMax - 60, position.y,60, position.height);
            position.width -= 65;
            EditorGUI.PropertyField(position, property,GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                if (property.objectReferenceValue != null)
                {
                    serializedObject = new SerializedObject(property.objectReferenceValue);
                }
            }

            if (property.objectReferenceValue != null)
            {
                if (serializedObject == null)
                {
                    serializedObject = new SerializedObject(property.objectReferenceValue);
                }
                valueProp = serializedObject.FindProperty("value");
            }
            else
            {
                serializedObject = null;
                valueProp = null;
                return;
            }
            

            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(valuePosition, valueProp,GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            
        }
    }
}