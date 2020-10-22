using UnityEngine;
using UnityEditor;


namespace GTVariable.Editor
{
    [UnityEditor.CustomEditor(typeof(BoolGameEvent))]
    public class BoolGameEventEditor 
        : GameEventEditor<BoolListener, BoolGameEvent, BoolEvent, bool>
    {

    }
}