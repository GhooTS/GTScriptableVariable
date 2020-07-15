﻿using UnityEditor;

namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(FloatGameEvent))]
    public class FloatGameEventEditor 
        : ParameterizedGameEventEditor<FloatGameEventListener, FloatGameEvent, FloatEvent, float>
    {
        protected override void DrawParameter()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            parameter = EditorGUILayout.FloatField("Parameter",parameter);
            EditorGUILayout.EndVertical();
        }
    }
}
