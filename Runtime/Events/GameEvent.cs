
// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

//Modify by Christopher Biernat
//Changes:
//EventListners property has been added

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/Event")]
public class GameEvent : ScriptableObject
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
