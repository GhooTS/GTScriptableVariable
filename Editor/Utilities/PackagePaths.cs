using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{
    public static class PackagePaths
    {
        private static string packagePath = string.Empty;

        public static string PackagePath
        {
            get
            {
                if (string.IsNullOrEmpty(packagePath) || AssetDatabase.IsValidFolder(packagePath) == false)
                {
                    var GUID = AssetDatabase.FindAssets("GTScriptableVariable");
                    packagePath = AssetDatabase.GUIDToAssetPath(GUID[0]);
                }

                return packagePath;
            }
        }
    }
}