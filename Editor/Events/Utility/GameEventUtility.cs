using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GTVariable.Editor
{

    public static class GameEventUtility
    {
        public static List<ListenerType> GetAssosiatedListenersInScene<ListenerType,EventType,ParameterType,GameEventType>(GameEventType target)
            where ListenerType : ParameterizedListener<GameEventType,EventType,ParameterType>
            where GameEventType : ParameterizedGameEvent<IParameterizedListener<EventType, ParameterType>,EventType,ParameterType>
            where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        {
            return GameObject.FindObjectsOfType<ListenerType>()
                    .Where(listener => listener.gameEvents
                    .Where(gameEvent => gameEvent == target)
                    .ToArray().Length > 0)
                    .ToList();
        }

        public static List<GameEventListener> GetAssosiatedListenersInScene(GameEvent target)
        {
            return GameObject.FindObjectsOfType<GameEventListener>()
                    .Where(listener => listener.gameEvents
                    .Where(gameEvent => gameEvent == target)
                    .ToArray().Length > 0)
                    .ToList();
        }


        public static void GetAssosiatedListenersInScene<ListenerType, EventType, ParameterType, GameEventType>(GameEventType target,List<ListenerType> listeners)
            where ListenerType : ParameterizedListener<GameEventType, EventType, ParameterType>
            where GameEventType : ParameterizedGameEvent<IParameterizedListener<EventType, ParameterType>, EventType, ParameterType>
            where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        {
            listeners.Clear();
            listeners.AddRange(GameObject.FindObjectsOfType<ListenerType>()
                                         .Where(listener => listener.gameEvents
                                         .Where(gameEvent => gameEvent == target)
                                         .ToArray().Length > 0)
                                         .ToList());
        }

        public static void GetAssosiatedListenersInScene(GameEvent target,List<GameEventListener> listeners)
        {
            listeners.Clear();
            listeners.AddRange(GameObject.FindObjectsOfType<GameEventListener>()
                                         .Where(listener => listener.gameEvents
                                         .Where(gameEvent => gameEvent == target)
                                         .ToArray().Length > 0)
                                         .ToList());
        }
    }
}