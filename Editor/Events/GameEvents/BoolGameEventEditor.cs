using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(BoolGameEvent))]
    public class BoolGameEventEditor 
        : GameEventEditor<BoolGameEvent, BoolEvent, bool>
    {
        public override void DrawParameter()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            parameter = EditorGUILayout.Toggle("Parameter", parameter);
            EditorGUILayout.EndVertical();
        }
    }
}