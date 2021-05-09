using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "GTVariable/Database")]
    public class VariableDatabase : ScriptableObject
    {
        [SerializeField] private List<VariableBase> persistentVariables = new List<VariableBase>();
        public List<VariableBase> PersistentVariables { get { return persistentVariables; } }
        private readonly List<VariableBase> runtimeVariables = new List<VariableBase>();
        [SerializeField] private List<GameEventBase> persistentGameEvents = new List<GameEventBase>();
        public List<GameEventBase> PersistentGameEvents { get { return persistentGameEvents; } }
        private readonly List<GameEventBase> runtimeGameEvents = new List<GameEventBase>();

        private readonly Dictionary<string, VariableBase> allVariables = new Dictionary<string, VariableBase>();
        private readonly Dictionary<string, GameEventBase> allGameEvents = new Dictionary<string, GameEventBase>();

        [SerializeField] private List<string> groups = new List<string>();

        [Header("Options")]
        public bool addTypeNameToKey = true;

        private bool init = false;


        private void OnDisable()
        {
            init = false;
        }

        public void Init()
        {
            if (init) return;

            runtimeGameEvents.Clear();
            runtimeVariables.Clear();

            allGameEvents.Clear();
            allVariables.Clear();

            foreach (var variable in persistentVariables)
            {
                AddVariable(variable);
            }

            foreach (var gameEvent in persistentGameEvents)
            {
                AddGameEvent(gameEvent);
            }

            init = true;
        }

        private bool AddVariable(VariableBase variable)
        {
            var key = GetKey(variable.name, variable.GetType());

            if (allVariables.ContainsKey(key) == false)
            {
                allVariables.Add(key, variable);
                return true;
            }
            else
            {
                Debug.LogWarning($"Variable with key {key} already exist");
            }

            return false;
        }

        private bool AddGameEvent(GameEventBase gameEvent)
        {
            var key = GetKey(gameEvent.name, gameEvent.GetType());

            if (allGameEvents.ContainsKey(key) == false)
            {
                allGameEvents.Add(key, gameEvent);
                return true;
            }
            else
            {
                Debug.LogWarning($"Game event with key {key} already exist");
            }

            return false;
        }


        public bool RemoveVariable(VariableBase variable)
        {
            if (runtimeVariables.Remove(variable))
            {
                allVariables.Remove(GetKey(variable.name, variable.GetType()));
                return true;
            }

            return false;
        }

        public bool RemoveGameEvent(GameEvent gameEvent)
        {
            if (runtimeGameEvents.Remove(gameEvent))
            {
                allGameEvents.Remove(GetKey(gameEvent.name, gameEvent.GetType()));
                return true;
            }

            return false;
        }

        public bool TryGetVariableByName<T>(string name,out T variable)
            where T : VariableBase
        {
            if(allVariables.TryGetValue(GetKey<T>(name), out VariableBase _variable) && _variable is T)
            {
                variable = _variable as T;
                return true;
            }

            variable = null;
            return false;
        }

        public bool TryGetGameEventByName<T>(string name, out T variable)
            where T : GameEventBase
        {
            if (allGameEvents.TryGetValue(GetKey<T>(name), out GameEventBase _variable) && _variable is T)
            {
                variable = _variable as T;
                return true;
            }

            variable = null;
            return false;
        }


        public T GetOrCreateVariable<T>(string name)
            where T : VariableBase
        {
            if (Application.isPlaying)
            {
                var key = GetKey<T>(name);

                if (allVariables.TryGetValue(key,out VariableBase variable))
                {
                    return variable as T;
                }

                variable = ScriptableObject.CreateInstance<T>();
                variable.name = name;
                if (AddVariable(variable))
                {
                    runtimeVariables.Add(variable);
                    return variable as T;
                }

                Destroy(variable);
            }

            return null;
        }

        public T GetOrCreateGameEvent<T>(string name)
            where T : GameEventBase
        {
            if (Application.isPlaying)
            {
                var key = GetKey<T>(name);

                if (allGameEvents.TryGetValue(key, out GameEventBase gameEvent))
                {
                    return gameEvent as T;
                }

                gameEvent = ScriptableObject.CreateInstance<T>();
                gameEvent.name = name;
                if (AddGameEvent(gameEvent))
                {
                    runtimeGameEvents.Add(gameEvent);
                    return gameEvent as T;
                }

                Destroy(gameEvent);
            }

            return null;
        }


        private string GetKey<T>(string name)
        {
            return addTypeNameToKey ? name + typeof(T).ToString() : name;
        }

        private string GetKey(string name,Type type)
        {
            return addTypeNameToKey ? name + type.ToString() : name;
        }
    }
}

