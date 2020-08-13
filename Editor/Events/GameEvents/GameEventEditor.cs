using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : GameEventEditorBase
    {
        private readonly List<GameEventListener> listeners = new List<GameEventListener>();
        private readonly List<bool> validionStates = new List<bool>();
        private GameEvent gameEvent;

        private void OnEnable()
        {
            gameEvent = target as GameEvent;
            UpdateListenersList();
        }

        

        public override void OnInspectorGUI()
        {
            Init();
            DrawOptions();
            DrawSubscribers(listeners,validionStates);
            EditorGUILayout.Space();
            if(currentSelected != -1 && currentSelected < listeners.Count) 
                DrawSelected(listeners[currentSelected],listeners[currentSelected].response);
        }

        private void CheckForResponseProblems()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                var valid = ListenerUtility.IsListenerValid(listeners[i]);
                if (i >= validionStates.Count)
                {
                    validionStates.Add(valid);
                }
                else
                {
                    validionStates[i] = valid;
                }
            }
        }

        public override void RaiseEvent()
        {
            gameEvent.Raise();
        }

        public override void UpdateListenersList()
        {
            GameEventUtility.GetAssosiatedListenersInScene(gameEvent, listeners);
            CheckForResponseProblems();
        }
    }
}