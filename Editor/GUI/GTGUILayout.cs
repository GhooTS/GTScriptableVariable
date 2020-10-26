using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace GTVariable.Editor
{
    public static class GTGUILayout
    {
        private static bool inital;
        private static GUIStyle plusButtonStyle;
        private static GUIStyle headerArrayStyle;
        private static GUIStyle arrayContentStyle;
        private static GUIContent plusIcon;
        private static GUIContent minusIcon;


        private static void Init()
        {
            if (inital == false)
            {
                plusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"));
                minusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
                plusButtonStyle = new GUIStyle
                {
                    imagePosition = ImagePosition.ImageOnly,
                    padding = new RectOffset(0, 0, 3, 0)
                };

                headerArrayStyle = new GUIStyle(EditorStyles.label)
                {
                    padding = new RectOffset(5, 0, 0, 2)
                };

                arrayContentStyle = new GUIStyle
                {
                    padding = new RectOffset(10, 10, 10, 10)
                };

                inital = true;
            }
        }

        public static void ArrayProperty(SerializedProperty property,string headerText)
        {
            if (property.isArray == false) return;
            Init();

            //Draw header
            EditorGUILayout.BeginHorizontal("RL Header");
            EditorGUILayout.LabelField(headerText, headerArrayStyle);
            if (GUILayout.Button(plusIcon, plusButtonStyle, GUILayout.MaxWidth(20)))
            {
                property.InsertArrayElementAtIndex(property.arraySize);
            }
            EditorGUILayout.EndHorizontal();

            //Draw content
            EditorGUILayout.BeginVertical("RL Background");
            EditorGUILayout.BeginVertical(arrayContentStyle);
            if (property.arraySize == 0)
            {
                EditorGUILayout.LabelField("List is Empty", GUILayout.MinHeight(EditorGUIUtility.singleLineHeight + 1));
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), GUIContent.none);
                if (GUILayout.Button(minusIcon, plusButtonStyle, GUILayout.MaxWidth(20)))
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