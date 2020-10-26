using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(StringGameEvent))]
    public class StringGameEventEditor 
        : GameEventEditor<StringListener, StringGameEvent, StringEvent, string>
    {
        public override void DrawParameter()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            parameter = EditorGUILayout.TextField("Parameter", parameter);
            EditorGUILayout.EndVertical();
        }
    }
}