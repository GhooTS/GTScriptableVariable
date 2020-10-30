namespace GTVariable
{
    [System.Serializable]
    public class ReadOnlyIntVariable : ReadOnlyVariable<IntVariable, int>
    {
        public ReadOnlyIntVariable(IntVariable variable) : base(variable)
        {

        }
    }
}