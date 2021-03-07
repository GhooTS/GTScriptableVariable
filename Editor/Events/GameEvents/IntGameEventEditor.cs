using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(IntGameEvent))]
    public class IntGameEventEditor 
        : GameEventEditor<IntGameEvent, IntEvent, int>
    {
        public override void DrawParameter()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            parameter = EditorGUILayout.IntField("Parameter", parameter);
            EditorGUILayout.EndVertical();
        }
    }
}