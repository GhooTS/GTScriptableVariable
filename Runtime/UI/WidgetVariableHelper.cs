namespace GTVariable
{
    public static class WidgetVariableHelper
    {

        public static T CreateVariableIfNeeded<T>(T variable, string variableName, bool create)
            where T : VariableBase
        {
            if (VariableRuntimeManager.Current != null)
            {
                if (create)
                {
                    return VariableRuntimeManager.Current.GetOrCreateVariable<T>(variableName);
                }
                else
                {
                    VariableRuntimeManager.Current.TryGetVariableByName(variableName, out variable);
                    return variable;
                }
            }

            return null;
        }
    }

}