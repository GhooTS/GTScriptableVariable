
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

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

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
            SerializedProperty useConstant = property.FindPropertyRelative("useConstant");
            SerializedProperty constantValue = property.FindPropertyRelative("constantValue");
            SerializedProperty variable = property.FindPropertyRelative("variable");


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
#if GT_ATTRIBUTES
            if (useConstant.boolValue == false)
            {
                menu.AddSeparator(string.Empty);
                menu.AddItem(new GUIContent("Quick View"), false, () => { GTQuickView.Editor.QuickView.Show(variable); });
            }
#endif
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            
            if (GUI.Button(buttonRect,string.Empty, popupStyle))
            {
                menu.DropDown(buttonRect);
            }

            
            EditorGUI.PropertyField(position,
                                    useConstant.boolValue ? constantValue : variable,
                                    GUIContent.none);
           


            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}