using UnityEditor;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(GameEventListener))]
    public class GameEventListenerEditor : EditorGroup<GameEventListener>
    {

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            DetachComponents();
        }

        protected override string GetComponentName(GameEventListener component)
        {
            return component.name;
        }

    }
}