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
                    plusIcon = new GUIContent(EditorGUIUtility.IconContent("d_Toolbar Plus More"));
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
            padding = new RectOffset(15, 0, 0, 0),
            margin = new RectOffset(1, 0, 0, 0),
            alignment = TextAnchor.MiddleLeft,
        };

        public static GUIStyle rowSelected = new GUIStyle(row)
        {
            normal = new GUIStyleState()
            {
                background = EditorGUIUtility.IconContent("selected").image as Texture2D,
                textColor = Color.white
            }
        };

        public static GUIStyle sideBarContent = new GUIStyle("OL box")
        {
            stretchHeight = true,
            padding = EditorStyles.inspectorDefaultMargins.padding,
            margin = EditorStyles.inspectorDefaultMargins.margin
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
            fontStyle = FontStyle.Bold,
            richText = true
        };


        private static Texture2D blueBar, greenBar, yellowBar;

        public static Texture2D GetSideBarItemTexture(SidebarItemType type)
        {
            switch (type)
            {
                case SidebarItemType.Channel:
                    return GetResourceTexture(blueBar, "blue.png");
                case SidebarItemType.Event:
                    return GetResourceTexture(yellowBar, "yellow.png");
                case SidebarItemType.Variable:
                    return GetResourceTexture(greenBar, "green.png");
                default:
                    return GetResourceTexture(blueBar, "blue.png");
            }

        }

        private static Texture2D GetResourceTexture(Texture2D source, string relativePath)
        {
            if (source == null) source = AssetDatabase.LoadAssetAtPath<Texture2D>($"{PackagePaths.PackagePath}/Editor/EditorResources/{relativePath}");
            return source;
        }
    }

}