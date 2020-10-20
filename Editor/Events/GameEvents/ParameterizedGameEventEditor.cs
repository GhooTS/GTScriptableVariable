using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace GTVariable.Editor
{

    public class ParameterizedGameEventEditor<ListenerType,GameEventType,EventType,ParameterType> : GameEventEditorBase
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        where ListenerType : ParameterizedListener<GameEventType, EventType,ParameterType>
        where GameEventType : ParameterizedGameEvent<ParameterizedListener<GameEventType, EventType, ParameterType>, EventType,ParameterType>
    {
        public ParameterType parameter = default;
        private readonly List<ListenerType> listeners = new List<ListenerType>();
        private readonly List<bool> listenerValid = new List<bool>();
        private GameEventType gameEvent;

        private void OnEnable()
        {
            Init();
            gameEvent = target as GameEventType;
            UpdateListenersList();
        }

        public override void OnInspectorGUI()
        {
            Init();
            DrawOptions();
            if(EditorApplication.isPlaying) DrawParameter();
            DrawSubscribers(listeners, listenerValid);
            EditorGUILayout.Space();
            if (currentSelected != -1 && currentSelected < listeners.Count)
            {
                DrawSelected(listeners[currentSelected], listeners[currentSelected].response);
            }

        }

        private void CheckForResponseProblems()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                var valid = ListenerUtility.IsListenerValid<ListenerType, EventType, ParameterType, GameEventType>(listeners[i]);
                if (i >= listenerValid.Count)
                {
                    listenerValid.Add(valid);
                }
                else
                {
                    listenerValid[i] = valid;
                }
            }
        }

        protected virtual void DrawParameter() { }

        public override void RaiseEvent()
        {
            gameEvent.Raise(parameter);
        }

        public override void UpdateListenersList()
        {
            GameEventUtility.GetAssosiatedListenersInScene<ListenerType, EventType, ParameterType, GameEventType>(gameEvent, listeners);
            CheckForResponseProblems();
        }
    }
}
