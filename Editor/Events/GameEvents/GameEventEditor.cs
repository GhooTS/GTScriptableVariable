using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : GameEventEditorBase<GameEvent>
    {
        public override void RaiseEvent()
        {
            gameEvent.Raise();
        }
    }
}