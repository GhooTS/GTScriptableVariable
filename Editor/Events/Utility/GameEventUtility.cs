using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GTVariable.Editor
{

    public static class GameEventUtility
    {
        /// <summary>
        /// Get all active and enable listener which subscribe to the target event in currently active scene
        /// </summary>
        /// <param name="target">target game event</param>
        /// <param name="listeners">the list to which the found listeners will be added</param>
        public static void GetAssosiatedListenersInScene(GameEventBase target,List<Listener> listeners)
        {
            listeners.AddRange( GameObject.FindObjectsOfType<Listener>()
                                        .Where(listener => listener.GetGameEvents()
                                        ?.Where(gameEvent => gameEvent == target).
                                        ToArray().Length > 0).
                                        ToList());
        }

        /// <summary>
        /// Get all active and enable listener which subscribe to the target event in currently active scene
        /// </summary>
        /// <param name="target">target game event</param>
        /// <returns></returns>
        public static List<ListenerType> GetAssosiatedListenersInScene<ListenerType,EventType,ParameterType,GameEventType>(GameEventType target)
            where ListenerType : Listener<GameEventType,EventType,ParameterType>
            where GameEventType : GameEvent<EventType,ParameterType>
            where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        {
            return GameObject.FindObjectsOfType<ListenerType>()
                    .Where(listener => listener.gameEvents
                    ?.Where(gameEvent => gameEvent == target)
                    .ToArray().Length > 0)
                    .ToList();
        }

        /// <summary>
        /// Get all active and enable listener which subscribe to the target event in currently active scene
        /// </summary>
        /// <param name="target">target game event</param>
        /// <returns></returns>
        public static List<GameEventListener> GetAssosiatedListenersInScene(GameEvent target)
        {
            return GameObject.FindObjectsOfType<GameEventListener>()
                    .Where(listener => listener.gameEvents
                    ?.Where(gameEvent => gameEvent == target)
                    .ToArray().Length > 0)
                    .ToList();
        }

        /// <summary>
        /// Get all active and enable listener which subscribe to the target event in currently active scene
        /// </summary>
        /// <param name="target">target game event</param>
        /// <param name="listeners">the list to which the found listeners will be added</param>
        public static void GetAssosiatedListenersInScene<ListenerType, EventType, ParameterType, GameEventType>(GameEventType target,List<ListenerType> listeners)
            where ListenerType : Listener<GameEventType, EventType, ParameterType>
            where GameEventType : GameEvent<EventType, ParameterType>
            where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        {
            listeners.AddRange(GameObject.FindObjectsOfType<ListenerType>()
                                         .Where(listener => listener.gameEvents
                                         ?.Where(gameEvent => gameEvent == target)
                                         .ToArray().Length > 0)
                                         .ToList());
        }

        /// <summary>
        /// Get all active and enable listener which subscribe to the target event in currently active scene
        /// </summary>
        /// <param name="target">target game event</param>
        /// <param name="listeners">the list to which the found listeners will be added</param>
        /// <returns></returns>
        public static void GetAssosiatedListenersInScene(GameEvent target,List<GameEventListener> listeners)
        {
            listeners.AddRange(GameObject.FindObjectsOfType<GameEventListener>()
                                         .Where(listener => listener.gameEvents
                                         ?.Where(gameEvent => gameEvent == target)
                                         .ToArray().Length > 0)
                                         .ToList());
        }
    }
}