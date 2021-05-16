using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GTVariable.Editor
{
    public class TypeTargetPair
    {
        public GameObject Target { get; set; }
        public Type Type { get; set; }

    }

    public class ComponenetCreator
    {
        private List<Type> types;



        public void UpdateComponentsTypesList<T>()
        {
            UpdateComponentsTypesList(typeof(T));
        }

        public void UpdateComponentsTypesList(Type baseType)
        {
            types?.Clear();
            if (baseType == null) return;

            types = AppDomain.CurrentDomain.GetAssemblies()
                                                    .SelectMany((assembly) => assembly.GetTypes())
                                                    .Where((type) => baseType.IsAssignableFrom(type)
                                                                  && type.IsAbstract == false
                                                                  && type.IsGenericType == false
                                                                  && type.IsClass)
                                                    .ToList();
        }


        public void ShowComponentMenu(Rect position,GameObject target)
        {
            var menu = new GenericMenu();

            foreach (var type in types)
            {
                menu.AddItem(new GUIContent(type.Name), false,AddComponent,new TypeTargetPair { Target = target, Type = type });
            }

            menu.DropDown(position);
        }

        public void ShowObjectMenu(GenericMenu.MenuFunction2 onTypeSelected)
        {
            if (types == null) return;

            var menu = new GenericMenu();

            foreach (var type in types)
            {
                menu.AddItem(new GUIContent(type.Name), false, onTypeSelected, type);
            }

            menu.ShowAsContext();
        }

        public void AddToMenu(GenericMenu menu,string path, GenericMenu.MenuFunction2 onTypeSelected)
        {
            foreach (var type in types)
            {
                menu.AddItem(new GUIContent($"{path}{type.Name}"), false, onTypeSelected, type);
            }
        }


        private void AddComponent(object boxedComponentAndTarget)
        {
            var typeTargetPair = boxedComponentAndTarget as TypeTargetPair;
            Undo.AddComponent(typeTargetPair.Target, typeTargetPair.Type);
        }

    }
}

