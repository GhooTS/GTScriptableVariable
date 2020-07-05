namespace GTVariable
{
    [UnityEngine.CreateAssetMenu(menuName = "ScriptableVars/Vars/Int")]
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