namespace GTVariable
{
    public interface IContainValue<T>
    {
        T GetValue();
        void SetValue(T value);
    }
}