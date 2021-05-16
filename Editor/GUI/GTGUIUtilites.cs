using System.Collections.Generic;
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


        public static bool Containe(Object target, SerializedProperty property)
        {
            if (property.isArray == false) return false;

            for (int i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).objectReferenceValue == target)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Containe(Object target, List<SerializedProperty> properties)
        {
            foreach (var property in properties)
            {
                if (Containe(target, property)) return true;
            }

            return false;
        }

        public static void AppendToArray(SerializedProperty from, SerializedProperty to, bool clearFromArray = true, bool copyNull = false)
        {
            if (from.isArray == false || to.isArray == false) return;

            for (int i = 0; i < from.arraySize; i++)
            {
                var objRef = from.GetArrayElementAtIndex(i).objectReferenceValue;
                if (copyNull || objRef != null) 
                {
                    to.InsertArrayElementAtIndex(to.arraySize);
                    to.GetArrayElementAtIndex(to.arraySize - 1).objectReferenceValue = objRef;
                }
            }

            if (clearFromArray) from.ClearArray();
        }

        public static void MoveToArray(SerializedProperty from, SerializedProperty to, SerializedProperty target)
        {
            if (from.isArray == false || to.isArray == false) return;

            var objRef = target.objectReferenceValue;

            if (objRef == null) return;

            var index = FindIndexInProperty(objRef, from);

            if(index != -1)
            {
                from.DeleteArrayElementAtIndex(index);
                from.DeleteArrayElementAtIndex(index);
                to.InsertArrayElementAtIndex(to.arraySize);
                to.GetArrayElementAtIndex(to.arraySize - 1).objectReferenceValue = objRef;
            }
        }
    }
}