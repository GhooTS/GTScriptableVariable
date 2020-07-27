using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    public class ComponentEnabledGruopController<T>
        where T : MonoBehaviour
    {
        public bool showMixedValue = false;
        public bool allEnabled = true;

        

        public void SetEnabled(int index,List<EditorGroupElement<T>> elements)
        {
            Undo.RecordObject(elements[index].Component, "Component Enabled");
            elements[index].Enabled = !elements[index].Enabled;
            UpdateMixedEnabled(elements);
        }
            

        public void UpdateMixedEnabled(List<EditorGroupElement<T>> elements)
        {
            showMixedValue = false;

            if (elements.Count < 2) return;

            var componentEnabled = elements[0].Enabled;

            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].Enabled != componentEnabled)
                {
                    showMixedValue = true;
                    return;
                }
            }

            if (showMixedValue == false && elements.Count != 0)
            {
                allEnabled = elements[0].Enabled;
            }
        }

        public void SwitchEnabledForAllComponents(List<EditorGroupElement<T>> elements)
        {
            allEnabled = !allEnabled;
            Undo.RecordObjects(elements.ConvertAll((element) => element.Component).ToArray(), "Components Enabled");
            elements.ForEach((element) => element.Enabled = allEnabled);
        }
    }
}

