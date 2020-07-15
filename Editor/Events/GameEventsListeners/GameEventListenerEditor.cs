using UnityEditor;

namespace GTVariable.Editor
{
    [CustomEditor(typeof(GameEventListener))]
    public class GameEventListenerEditor : EditorGroup<Listener>
    {

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            DetachComponents();
        }

        protected override string GetComponentName(Listener component)
        {
            return component.name;
        }

    }
}