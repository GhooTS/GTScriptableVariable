using UnityEditor;

namespace GTVariable.Editor
{

    [CustomPropertyDrawer(typeof(BoolVariable))]
    public class BoolVariablePropertyDrawer : VariablePropertyDrawer
    {
        protected override void Init()
        {
            inlineDrawer.inlineWidth = 20f;
        }
    }
}