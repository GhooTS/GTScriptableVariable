using System.Runtime.Remoting.Messaging;

namespace GTVariable.Editor
{
    /// <summary>
    /// Validation state for <seealso cref="UnityEngine.Events.UnityEventBase"/>
    /// </summary>
    public enum ListenerValidionState
    {
        /// <summary>
        /// event is valide
        /// </summary>
        Valid,
        /// <summary>
        /// event missing target
        /// </summary>
        MissingTarget,
        /// <summary>
        /// event missing method name
        /// </summary>
        MissingMethod
    }

    public static class ListenerUtility
    {

        /// <summary>
        /// Validate listener
        /// </summary>
        /// <param name="listener"></param>
        /// <returns>returns false if one of the <seealso cref="GameEventListener.response"/> missing target or method</returns>
        public static bool IsListenerValid(GameEventListener listener)
        {
            for (int i = 0; i < listener.response.GetPersistentEventCount(); i++)
            {
                if (ValidedResponse(listener.response, i) != ListenerValidionState.Valid) return false;
            }

            return true;
        }


        /// <summary>
        /// Validate listener
        /// </summary>
        /// <param name="listener"></param>
        /// <returns>returns false if one of the <seealso cref="ParameterizedListener{GameEventType, EventType, ParameterType}.response"/> missing target or method</returns>
        public static bool IsListenerValid<ListenerType,EventType,ParameterType,GameEventType>(ListenerType listener)
            where ListenerType : ParameterizedListener<GameEventType,EventType,ParameterType>
            where GameEventType : ParameterizedGameEvent<ParameterizedListener<GameEventType, EventType, ParameterType>, EventType,ParameterType>
            where EventType : UnityEngine.Events.UnityEvent<ParameterType>
        {
            for (int i = 0; i < listener.response.GetPersistentEventCount(); i++)
            {
                if (ValidedResponse(listener.response, i) != ListenerValidionState.Valid) return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="index"></param>
        /// <returns>return one of the <seealso cref="ListenerValidionState"/></returns>
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