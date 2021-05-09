using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{
    public static class GTGUIStyles
    {
        private static GUIContent plusIcon = null;
        private static GUIContent minusIcon = null;

        public static GUIContent PlusIcon 
        { 
            get 
            { 
                if(plusIcon == null)
                {
                    plusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"));
                }

                return plusIcon;
            } 
        }

        public static GUIContent MinusIcon
        {
            get
            {
                if (minusIcon == null)
                {
                    minusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"));
                }

                return minusIcon;
            }
        }


        public static GUIStyle plusButtonStyle = new GUIStyle
        {
            imagePosition = ImagePosition.ImageOnly,
            padding = new RectOffset(0, 0, 3, 0)
        };

        public static GUIStyle headerArrayStyle = new GUIStyle(EditorStyles.label)
        {
            padding = new RectOffset(5, 0, 0, 2)
        };

        public static GUIStyle arrayContentStyle = new GUIStyle
        {
            padding = new RectOffset(10, 10, 10, 10)
        };

        public static GUIStyle row = new GUIStyle("Label")
        {
            padding = new RectOffset(15,0,0,0),
            margin = new RectOffset(1,0,0,0),
        };

        public static GUIStyle rowSelected = new GUIStyle(row)
        {
            normal = new GUIStyleState()
            {
                background = EditorGUIUtility.IconContent("selected").image as Texture2D
            }
        };

        public static GUIStyle sideBarContent = new GUIStyle("OL box")
        {
            stretchHeight = true,
            padding = new RectOffset(0, 0, 2, 2),
            margin = new RectOffset(0, 0, 0, 0),
        };

        public static GUIStyle sideBarHeader = new GUIStyle("OL Title")
        {
            padding = new RectOffset(5, 5, 2, 0),
            margin = new RectOffset(0, 0, 0, 0),
            fixedHeight = 25
        };

        public static GUIStyle sideBarElementLine = new GUIStyle()
        {
            stretchWidth = true,
            fixedHeight = 2,
            normal = new GUIStyleState()
            {
                background = Texture2D.blackTexture
            }
        };

        public static GUIStyle labelCenter = new GUIStyle("Label")
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

    }

}