namespace GTVariable.Editor
{
    public static class ValidationMessage
    {
        public static string GetMessage(ListenerValidionState validionState)
        {
            switch (validionState)
            {
                case ListenerValidionState.Valid:
                    return "Valid";
                case ListenerValidionState.MissingTarget:
                    return "Listener missing event target";
                case ListenerValidionState.MissingMethod:
                    return "Listener missing method target";
                default:
                    return "listener is invalid";
            }
        }
    }
}