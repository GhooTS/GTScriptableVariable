

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GTVariable
{

    /// <summary>
    /// Zero argument game event
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableVars/Events/Event")]
    public class GameEvent : GameEventBase
    {
        /// <summary>
        /// List of listeners that subscribe to this game event
        /// </summary>
        public List<Listener> EventListners { get { return eventListners; } }
        /// <summary>
        /// This action is invoke whenever game event is Raise 
        /// </summary>
        public UnityAction OnEventRaise { get; private set; }
        private readonly List<Listener> eventListners = new List<Listener>();

        public void Raise()
        {
            OnEventRaise?.Invoke();
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