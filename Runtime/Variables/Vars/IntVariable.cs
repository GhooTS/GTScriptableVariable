namespace GTVariable
{
    [UnityEngine.CreateAssetMenu(menuName = "ScriptableVars/Vars/Int")]
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