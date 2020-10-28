using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    /// <summary>
    /// Derive from this class to create custom variable property drawer
    /// </summary>
    public class VariableInlineDrawer
    {
        public RectOffset wrapperPadding = new RectOffset(-5, -5, -7, -7);
        private SerializedProperty valueProp;
        private SerializedObject serializedObject;

        public bool HasProperty()
        {
            return serializedObject != null;
        }

        public void Update(SerializedProperty property)
        {
            if (property.objectReferenceValue != null && (HasProperty() == false || serializedObject.targetObject != property.objectReferenceValue))
            {
                serializedObject = new SerializedObject(property.objectReferenceValue);
                valueProp = serializedObject.FindProperty("value");
            }
        }

        public Rect Reserve(ref Rect position)
        {
            if (HasProperty() == false) return Rect.zero;

            position.height = EditorGUIUtility.singleLineHeight;
            var output = new Rect(position);
            output.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            output.height = EditorGUI.GetPropertyHeight(valueProp, GUIContent.none);
            return output;
        }

        public void DrawWrapper(ref Rect position)
        {
            if (HasProperty() == false) return;
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);
            position = wrapperPadding.Add(position);
        }

        public void DrawProperty(SerializedProperty property, Rect position)
        {
            if (HasProperty() == false) return;

            if (property.objectReferenceValue != null)
            {
                if (serializedObject == null)
                {
                    Update(property);
                }

                serializedObject.UpdateIfRequiredOrScript();

                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, valueProp);
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
        }

        public float GetHeight()
        {
            return HasProperty() ? EditorGUI.GetPropertyHeight(valueProp) + EditorGUIUtility.standardVerticalSpacing - wrapperPadding.vertical : 0;
        }
    }
}