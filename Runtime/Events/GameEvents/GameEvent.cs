

using System.Collections.Generic;
using UnityEngine;

namespace GTVariable
{

    [CreateAssetMenu(menuName = "ScriptableVars/Events/Event")]
    public class GameEvent : GameEventBase
    {

        public List<GameEventListener> EventListners { get { return eventListners; } }
        private readonly List<GameEventListener> eventListners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = eventListners.Count - 1; i >= 0; i--)
            {
                eventListners[i].OnEventRised();
            }
        }

        public void RegisterListener(GameEventListener listner)
        {
            eventListners.Add(listner);
        }

        public void UnRegisterListener(GameEventListener listner)
        {
            eventListners.Remove(listner);
        }
    }
}