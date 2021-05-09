using UnityEditor;


namespace GTVariable.Editor
{
    [CustomPropertyDrawer(typeof(IntVariable))]
    public class IntVariablePropertyDrawer : VariablePropertyDrawer<IntVariable,int>
    {

    }
}