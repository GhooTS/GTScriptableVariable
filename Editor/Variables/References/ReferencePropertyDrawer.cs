using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{

    /// <summary>
    /// Derive from this class to create custom reference property drawer
    /// </summary>
    public class ReferencePropertyDrawer<T,ArgType> : PropertyDrawer
        where T : Variable<ArgType>
    {

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;
        private SerializedProperty value;
        private SerializedProperty useConstant;
        private readonly VariableInlineDrawer inlineDrawer = new VariableInlineDrawer();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property)
                + (useConstant == null || useConstant.boolValue ? 0 : inlineDrawer.GetHeight());
        } 

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            // Get properties
            useConstant = property.FindPropertyRelative("useConstant");
            value = property.FindPropertyRelative(useConstant.boolValue ? "constantValue" : "variable");

            Rect inlinePosition = new Rect();

            if (useConstant.boolValue == false)
            {
                inlineDrawer.Update(value);
                inlineDrawer.DrawWrapper(ref position);
                inlinePosition = inlineDrawer.Reserve(ref position);
            }

            label.text = property.displayName;
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;
            
            if (GUI.Button(buttonRect, string.Empty, popupStyle))
            {
                ShowContentMenuAsContext(property);
            }

            if (useConstant.boolValue)
            {
                EditorGUI.PropertyField(position, value, GUIContent.none);
            }
            else
            {
                value.objectReferenceValue = EditorGUI.ObjectField(position, value.objectReferenceValue, typeof(T), true);
                inlineDrawer.DrawProperty(value,inlinePosition);
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        private void ShowContentMenuAsContext(SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Use Constant"), false, () =>
            {
                useConstant.boolValue = true;
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.AddItem(new GUIContent("Use Variable"), false, () =>
            {
                useConstant.boolValue = false;
                property.serializedObject.ApplyModifiedProperties();
            });

            menu.ShowAsContext();
        }
    }
}