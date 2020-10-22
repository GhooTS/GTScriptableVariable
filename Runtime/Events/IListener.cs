namespace GTVariable
{
    public interface IListener<EventType, ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        void OnEventRised(ParameterType value);
    }
}
