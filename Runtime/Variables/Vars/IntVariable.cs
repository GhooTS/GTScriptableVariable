namespace GTVariable
{
    public class IntVariable : Variable<int>
    {
        public void Increament()
        {
            SetValue(value + 1);
        }

        public void Decreament()
        {
            SetValue(value - 1);
        }

    }
}