

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GTVariable
{

    /// <summary>
    /// Zero argument game event
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Event")]
    public class GameEvent : GameEventBase
    {
        public List<Listener> EventListners { get { return eventListners; } }
        private readonly List<Listener> eventListners = new List<Listener>();

        public void Raise()
        {
            for (int i = eventListners.Count - 1; i >= 0; i--)
            {
                eventListners[i].OnEventRised();
            }
        }

        public override void RegisterListener(Listener listner)
        {
            eventListners.Add(listner);
        }

        public override void UnRegisterListener(Listener listner)
        {
            eventListners.Remove(listner);
        }

        
    }
}