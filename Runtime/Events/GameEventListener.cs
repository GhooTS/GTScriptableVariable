
// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

//Modify by Christopher Biernat
//Changes:
//A name has been added for better identyfication in the editor
//GameEvent has been change from variable to array, so gameEvents with same response can be group

using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour
{
#if UNITY_EDITOR
    new public string name;
#endif
    public GameEvent[] gameEvents;
    public UnityEvent Response;

    public void OnEventRised()
    {
        Response?.Invoke();
    }

    private void OnEnable()
    {
        foreach (var gameEvent in gameEvents)
        {
            gameEvent.RegisterListener(this);
        }
        
    }

    private void OnDisable()
    {
        foreach (var gameEvent in gameEvents)
        {
            gameEvent.UnRegisterListener(this);
        }
    }
}
