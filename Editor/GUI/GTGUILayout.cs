using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace GTVariable.Editor
{
    public static partial class GTGUILayout
    {
        public static void ArrayProperty(SerializedProperty property,string headerText)
        {
            if (property.isArray == false) return;

            //Draw header
            EditorGUILayout.BeginHorizontal("RL Header");
            EditorGUILayout.LabelField(headerText, GTGUIStyles.headerArrayStyle);
            if (GUILayout.Button(GTGUIStyles.PlusIcon, GTGUIStyles.plusButtonStyle, GUILayout.MaxWidth(20)))
            {
                property.InsertArrayElementAtIndex(property.arraySize);
            }
            EditorGUILayout.EndHorizontal();

            //Draw content
            EditorGUILayout.BeginVertical("RL Background");
            EditorGUILayout.BeginVertical(GTGUIStyles.arrayContentStyle);
            if (property.arraySize == 0)
            {
                EditorGUILayout.LabelField("List is Empty", GUILayout.MinHeight(EditorGUIUtility.singleLineHeight + 1));
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), GUIContent.none);
                if (GUILayout.Button(GTGUIStyles.MinusIcon, GTGUIStyles.plusButtonStyle, GUILayout.MaxWidth(20)))
                {
                    var deleteTwice = property.GetArrayElementAtIndex(i).objectReferenceValue != null;
                    property.DeleteArrayElementAtIndex(i);
                    if (deleteTwice) property.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
                if(i < property.arraySize - 1)
                {
                    EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

    }
}