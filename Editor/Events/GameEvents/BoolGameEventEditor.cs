using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(BoolGameEvent))]
    public class BoolGameEventEditor 
        : ParameterizedGameEventEditor<BoolListener, BoolGameEvent, BoolEvent, bool>
    {

    }
}