using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{
    public static class GTGUIUtilites
    {
        public static int FindIndexInProperty(Object target, SerializedProperty property)
        {
            if (property.isArray == false) return -1;

            for (int i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).objectReferenceValue == target)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}