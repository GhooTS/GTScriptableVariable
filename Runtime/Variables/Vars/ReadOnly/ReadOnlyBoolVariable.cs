namespace GTVariable
{
    [System.Serializable]
    public class ReadOnlyBoolVariable : ReadOnlyVariable<BoolVariable, bool>
    {
        public ReadOnlyBoolVariable(BoolVariable variable) : base(variable)
        {

        }
    }
}