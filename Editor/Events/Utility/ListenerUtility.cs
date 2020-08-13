using System.Runtime.Remoting.Messaging;

namespace GTVariable.Editor
{
    public enum ListenerValidionState
    {
        Valid,
        MissingTarget,
        MissingMethod
    }

    public static class ListenerUtility
    {
        public static bool IsListenerValid(GameEventListener listener)
        {
            for (int i = 0; i < listener.response.GetPersistentEventCount(); i++)
            {
                if (ValidedResponse(listener.response, i) != ListenerValidionState.Valid) return false;
            }

            return true;
        }


        public static bool IsListenerValid<ListenerType,EventType,ParameterType,GameEventType>(ListenerType listener)
            where ListenerType : ParameterizedListener<GameEventType,EventType,ParameterType>
            where GameEventType : ParameterizedGameEvent<IParameterizedListener<EventType,ParameterType>,EventType,ParameterType>
            where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        {
            for (int i = 0; i < listener.response.GetPersistentEventCount(); i++)
            {
                if (ValidedResponse(listener.response, i) != ListenerValidionState.Valid) return false;
            }

            return true;
        }


        public static ListenerValidionState ValidedResponse(UnityEngine.Events.UnityEventBase response,int index)
        {
           
            if (response.GetPersistentTarget(index) == null)
                return ListenerValidionState.MissingTarget;
            if (string.IsNullOrEmpty(response.GetPersistentMethodName(index)))
                return ListenerValidionState.MissingMethod;

            return ListenerValidionState.Valid;
        }
    }
}