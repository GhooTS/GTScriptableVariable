namespace GTVariable
{
    public interface IRaisable<EventType,ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        void OnEventRised(ParameterType value);
    }
}
