using UnityEditor;

namespace GTVariable.Editor
{
    [CustomPropertyDrawer(typeof(StringVariable))]
    public class StringVariablePropertyDrawer : VariablePropertyDrawer
    {
        protected override void Init()
        {
            inlineDrawer.propertyPosition = VariableInlineDrawer.PropertyPosition.Under;
        }
    }
}