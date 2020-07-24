using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{
    public class ComponentEnabledGruopController
    {
        public bool showMixedValue = false;
        public bool allEnabled = true;

        

        public void UpdateMixedEnabled<T>(List<EditorGroupElement<T>> elements)
            where T : MonoBehaviour
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

            
        }

        public void SetEnabledForAllComponents<T>(List<EditorGroupElement<T>> elements)
            where T : MonoBehaviour
        {
            Undo.RecordObjects(elements.ConvertAll((element) => element.Component).ToArray(), "Components Enabled");
            elements.ForEach((element) => element.Enabled = allEnabled);
        }
    }
}

