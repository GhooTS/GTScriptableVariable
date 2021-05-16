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
                if(_current == null)
                {
                    _current = FindObjectOfType<VariableRuntimeManager>();
                }
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

        public bool TryGetVariableByName<T>(string name, out T variable,string channelName = "")
            where T : VariableBase
        {
            return database.TryGetVariableByName(name, out variable, channelName);
        }

        public bool TryGetGameEventByName<T>(string name, out T variable, string channelName = "")
            where T : GameEventBase
        {
            return database.TryGetGameEventByName(name, out variable, channelName);
        }

        public T GetOrCreateVariable<T>(string name,string channelName = "")
            where T : VariableBase
        {
            return database.GetOrCreateVariable<T>(name, channelName);
        }

        public T GetOrCreateGameEvent<T>(string name, string channelName = "")
            where T : GameEventBase
        {
            return database.GetOrCreateGameEvent<T>(name,channelName);
        }

        public bool TryGetChannel(string name,out Channel channel)
        {
            return database.TryGetChannel(name,out channel);
        }

        public Channel GetOrCreateChannel(string name)
        {
            return database.GetOrCreateChannel(name);
        }

    }
}