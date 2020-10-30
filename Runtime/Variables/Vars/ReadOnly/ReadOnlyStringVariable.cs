namespace GTVariable
{
    [System.Serializable]
    public class ReadOnlyStringVariable : ReadOnlyVariable<StringVariable, string>
    {
        public ReadOnlyStringVariable(StringVariable variable) : base(variable)
        {

        }
    }
}