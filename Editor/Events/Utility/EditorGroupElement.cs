using UnityEditor;
using UnityEngine;

namespace GTVariable.Editor
{
    public class EditorGroupElement<T>
        where T : MonoBehaviour
    {
        public EditorGroupElement(T component,string name = "",bool useDefaultNameIfEmpty = true)
        {
            Component = component;
            SerializedObject = new SerializedObject(component);
            Foldout = false;
            SetName(name, useDefaultNameIfEmpty);
        }

        public string Name { get; private set; }
        public T Component { get; set; }
        public SerializedObject SerializedObject { get; set; }
        public bool Attach { get; private set; }
        public bool ShouldBeDetach { get; set; } = false;
        public bool Foldout { get; set; }
        public bool Enabled 
        { 
            get { return Component.enabled; } 
            set 
            {
                Component.enabled = value; 
            } 
        }


        public void ChangeComponent(T component,string name = "",bool useDefaultNameIfEmpty = true)
        {
            if (HasComponent() == false || component != Component)
            {
                Component = component;
                SerializedObject = new SerializedObject(component);
            }

            SetName(name,useDefaultNameIfEmpty);
        }

        public void SetAttach(bool attach)
        {
            if (HasComponent() == false) return;

            if (ShouldBeDetach)
            {
                Component.hideFlags = HideFlags.None;
                Attach = false;
                return;
            }

            Component.hideFlags = attach ? HideFlags.HideInInspector : HideFlags.None;
            Attach = attach;
        }

        public void Update()
        {
            SerializedObject.Update();
        }

        public void ApplyModifiedProperties()
        {
            SerializedObject.ApplyModifiedProperties();
        }

        public void SetName(string name,bool useDefaultNameIfEmpty = true)
        {
            Name = name;

            if (useDefaultNameIfEmpty && string.IsNullOrEmpty(Name)) 
            {
                Name = typeof(T).Name;
            }
        }

        public bool HasComponent()
        {
            return Component != null;
        }
    }
}