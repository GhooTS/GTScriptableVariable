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

    public class ComponenetCreator<T>
        where T : MonoBehaviour
    {
        private List<Type> componentTypes;



        public void UpdateComponentsTypesList()
        {
            var baseType = typeof(T);
            componentTypes = AppDomain.CurrentDomain.GetAssemblies()
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

            foreach (var type in componentTypes)
            {
                menu.AddItem(new GUIContent(type.Name), false,AddComponent,new TypeTargetPair { Target = target, Type = type });
            }

            menu.DropDown(position);
        }


        private void AddComponent(object boxedComponentAndTarget)
        {
            var typeTargetPair = boxedComponentAndTarget as TypeTargetPair;
            Undo.AddComponent(typeTargetPair.Target, typeTargetPair.Type);
        }


        
    }
}

