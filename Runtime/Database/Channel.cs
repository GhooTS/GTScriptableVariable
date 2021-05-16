using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GTVariable
{
    [Serializable]
    public class Channel : INameable
    {
        [SerializeField] private List<GameEventBase> gameEvents = new List<GameEventBase>();
        [SerializeField] private List<VariableBase> variables = new List<VariableBase>();

        [SerializeField] private string name;
        private readonly Dictionary<string, VariableBase> variablesMap = new Dictionary<string, VariableBase>();
        private readonly Dictionary<string, GameEventBase> gameEventsMap = new Dictionary<string, GameEventBase>();

        [NonSerialized] private bool init = false;
        public string Name { get => name; set => name = value; }

        public void InitChannel()
        {
            if (init) return;

            variablesMap.Clear();
            gameEventsMap.Clear();

            foreach (var variable in variables)
            {
                AddVariable(variable);
            }

            foreach (var gameEvent in gameEvents)
            {
                AddGameEvent(gameEvent);
            }

            init = true;
        }

        private bool AddVariable(VariableBase variable)
        {
            if (variablesMap.ContainsKey(variable.name) == false)
            {
                variablesMap.Add(variable.name, variable);
                return true;
            }
            else
            {
                Debug.LogWarning($"channel({name}) : Variable with name {variable.name} already exist!");
            }

            return false;
        }

        private bool AddGameEvent(GameEventBase gameEvent)
        {
            if (gameEvent == null) return false;


            if (gameEventsMap.ContainsKey(gameEvent.name) == false)
            {
                gameEventsMap.Add(gameEvent.name, gameEvent);
                return true;
            }
            else
            {
                Debug.LogWarning($"channel({name}) : Game event with key {gameEvent.name} already exist!");
            }

            return false;
        }


        public bool RemoveVariable(VariableBase variable)
        {
            InitChannel();

            if (variables.Contains(variable))
            {
                variablesMap.Remove(variable.name);
                return true;
            }

            return false;
        }

        public bool RemoveGameEvent(GameEvent gameEvent)
        {
            InitChannel();

            if (gameEvents.Contains(gameEvent))
            {
                gameEventsMap.Remove(gameEvent.name);
                return true;
            }

            return false;
        }

        public bool TryGetVariableByName<T>(string name, out T variable)
            where T : VariableBase
        {
            InitChannel();

            if (variablesMap.TryGetValue(name, out VariableBase _variable) && _variable is T)
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
            InitChannel();

            if (gameEventsMap.TryGetValue(name, out GameEventBase _variable) && _variable is T)
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
           

            if (Application.isPlaying && string.IsNullOrEmpty(name) == false)
            {
                InitChannel();

                if (variablesMap.TryGetValue(name, out VariableBase variable))
                {
                    return variable as T;
                }

                variable = ScriptableObject.CreateInstance<T>();
                variable.name = name;
                if (AddVariable(variable))
                {
                    return variable as T;
                }

                GameObject.Destroy(variable);
            }

            return null;
        }

        public T GetOrCreateGameEvent<T>(string name)
            where T : GameEventBase
        {

            if (Application.isPlaying && string.IsNullOrEmpty(name) == false)
            {
                InitChannel();

                if (gameEventsMap.TryGetValue(name, out GameEventBase gameEvent))
                {
                    return gameEvent as T;
                }

                gameEvent = ScriptableObject.CreateInstance<T>();
                gameEvent.name = name;
                if (AddGameEvent(gameEvent))
                {
                    return gameEvent as T;
                }

                GameObject.Destroy(gameEvent);
            }

            return null;
        }


        public bool ContaineVariable(string name)
        {

            if (Application.isPlaying)
            {
                InitChannel();

                return variablesMap.ContainsKey(name);
            }
            else
            {
                return variables.Find(x => x.Name == name);
            }
        }

        public bool ContaineGameEvent(string name)
        {
            if (Application.isPlaying)
            {
                InitChannel();

                return gameEventsMap.ContainsKey(name);
            }
            else
            {
                return gameEvents.Find(x => x.Name == name);
            }
        }


        public void DestroyNotPersistendObjects()
        {
            var gameEvents = gameEventsMap.Values.Where(x => this.gameEvents.Contains(x) == false).ToList();
            var variables = variablesMap.Values.Where(x => this.variables.Contains(x) == false).ToList();

            for (int i = 0; i < gameEvents.Count; i++)
            {
                gameEventsMap.Remove(gameEvents[i].Name);
                GameObject.Destroy(gameEvents[i]);
            }

            for (int i = 0; i < variables.Count; i++)
            {
                variablesMap.Remove(variables[i].Name);
                GameObject.Destroy(variables[i]);
            }
        }


        public bool IsChannelEmpty()
        {
            return variables.Count + variablesMap.Count + gameEvents.Count + gameEventsMap.Count == 0;
        }

    }
}

