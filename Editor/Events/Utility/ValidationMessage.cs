namespace GTVariable.Editor
{
    public static class ValidationMessage
    {
        /// <returns>returns proper validation message base on the <seealso cref="ListenerValidionState"/></returns>
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