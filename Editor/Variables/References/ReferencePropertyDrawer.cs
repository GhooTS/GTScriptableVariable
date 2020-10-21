using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{
    public class ReferencePropertyDrawer : PropertyDrawer
    {

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;
        private SerializedProperty value;
        private SerializedProperty useConstant;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return  useConstant == null || useConstant.boolValue ? EditorGUI.GetPropertyHeight(property) : EditorGUI.GetPropertyHeight(value);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);



            EditorGUI.BeginChangeCheck();

            // Get properties
            useConstant = property.FindPropertyRelative("useConstant");
            value = property.FindPropertyRelative(useConstant.boolValue ? "constantValue" : "variable");


            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

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

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            
            if (GUI.Button(buttonRect,string.Empty, popupStyle))
            {
                menu.DropDown(buttonRect);
            }


            EditorGUI.PropertyField(position, value, GUIContent.none);
           


            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}