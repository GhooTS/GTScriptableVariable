using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(StringGameEvent))]
    public class StringGameEventEditor 
        : GameEventEditor<StringListener, StringGameEvent, StringEvent, string>
    {

    }
}