using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor.Utility
{
    public static class WindowPrefs
    {
        private static readonly string xMin = "xMin";
        private static readonly string xMax = "xMax";
        private static readonly string yMin = "yMin";
        private static readonly string yMax = "yMax";

        /// <summary>
        /// Save current window position
        /// </summary>
        /// <param name="windowName">use as the key in editor prefs</param>
        /// <param name="position"></param>
        public static void SavePosition(string windowName, Rect position)
        {
            EditorPrefs.SetFloat($"{windowName}{xMin}", position.xMin);
            EditorPrefs.SetFloat($"{windowName}{xMax}", position.xMax);
            EditorPrefs.SetFloat($"{windowName}{yMin}", position.yMin);
            EditorPrefs.SetFloat($"{windowName}{yMax}", position.yMax);
        }

        /// <summary>
        /// Load save position base on editor window name
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static Rect LoadPosition(string windowName)
        {
            var output = new Rect();

            if (EditorPrefs.HasKey($"{windowName}{xMin}")) output.xMin = EditorPrefs.GetFloat($"{windowName}{xMin}");
            if (EditorPrefs.HasKey($"{windowName}{xMax}")) output.xMax = EditorPrefs.GetFloat($"{windowName}{xMax}");
            if (EditorPrefs.HasKey($"{windowName}{yMin}")) output.yMin = EditorPrefs.GetFloat($"{windowName}{yMin}");
            if (EditorPrefs.HasKey($"{windowName}{yMax}")) output.yMax = EditorPrefs.GetFloat($"{windowName}{yMax}");

            return output;
        }
    }
}