using UnityEngine;

namespace GTVariable
{
    public abstract class Listener : MonoBehaviour
    {
#if UNITY_EDITOR
        new public string name;
#endif

    }
}