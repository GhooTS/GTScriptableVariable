using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(StringGameEvent))]
    public class StringGameEventEditor 
        : ParameterizedGameEventEditor<StringListener, StringGameEvent, StringEvent, string>
    {

    }
}