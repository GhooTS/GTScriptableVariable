using UnityEngine;
using UnityEngine.Assertions;

namespace GTVariable
{
    [DefaultExecutionOrder(-50)]
    public class VariableRuntimeManager : MonoBehaviour
    {
        [SerializeField] private VariableDatabase database;
        public VariableDatabase Database { get { return database; } }

        private static VariableRuntimeManager _current;
        public static VariableRuntimeManager Current
        {
            get
            {
                Assert.AreNotEqual(null, _current, "Instance of Variable Runtime Manager is not present in scene!");
                return _current;
            }
        }


        private void Awake()
        {
            if (_current != null) Destroy(this);


            _current = this;
            if(database == null)
            {
                database = ScriptableObject.CreateInstance<VariableDatabase>();
                database.name = "(Created) Runtime-Database";
            }

            database.Init();
        }

        public bool TryGetVariableByName<T>(string name, out T variable)
            where T : VariableBase
        {
            return database.TryGetVariableByName<T>(name, out variable);
        }

        public bool TryGetGameEventByName<T>(string name, out T variable)
            where T : GameEventBase
        {
            return database.TryGetGameEventByName(name, out variable);
        }


        public T GetOrCreateVariable<T>(string name)
            where T : VariableBase
        {
            return database.GetOrCreateVariable<T>(name);
        }

        public T GetOrCreateGameEvent<T>(string name)
            where T : GameEventBase
        {
            return database.GetOrCreateGameEvent<T>(name);
        }

    }
}