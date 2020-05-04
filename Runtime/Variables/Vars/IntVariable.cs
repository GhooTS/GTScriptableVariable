namespace GTVariable
{
    [UnityEngine.CreateAssetMenu(menuName = "Variables/Int")]
    public class IntVariable : Variable<int>
    {
        public bool resetValueOnEnable;

        private void OnEnable()
        {
            if (resetValueOnEnable)
            {
                value = 0;
            }
        }

        public void Increament(bool sendChanageEvent = false)
        {
            value++;
            if (sendChanageEvent)
            {
                onValueChange?.Invoke();
            }
        }

        public void Decreament(bool sendChanageEvent = false)
        {
            value--;
            if (sendChanageEvent)
            {
                onValueChange?.Invoke();
            }
        }

    }
}