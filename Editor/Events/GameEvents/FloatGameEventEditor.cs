using UnityEditor;

namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(FloatGameEvent))]
    public class FloatGameEventEditor : GameEventEditor<FloatListener, FloatGameEvent, FloatEvent, float>
    {
        public override void DrawParameter()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            parameter = EditorGUILayout.FloatField("Parameter", parameter);
            EditorGUILayout.EndVertical();
        }
    }
}
