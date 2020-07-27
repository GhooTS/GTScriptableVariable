namespace GTVariable
{
    public interface IParameterizedListener<EventType,ParameterType>
        where EventType : UnityEngine.Events.UnityEvent<ParameterType>
    {
        void OnEventRised(ParameterType value);
    }
}
