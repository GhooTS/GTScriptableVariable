using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GTVariable
{
    [CreateAssetMenu(menuName = "GTVariable/Database")]
    public class VariableDatabase : ScriptableObject
    {
        [SerializeField] private Channel defaultChannel = new Channel() 
        {
            Name = "Default Channel"
        };
        [SerializeField] private List<Channel> channels = new List<Channel>();
        private Dictionary<string, Channel> channelMap = new Dictionary<string, Channel>();

        [NonSerialized] private bool init = false;


        private void OnDisable()
        {
            init = false;
        }

        public void Init()
        {
            if (init) return;

            defaultChannel.InitChannel();

            foreach (var channel in channels)
            {
                if (channelMap.ContainsKey(channel.Name) == false)
                {
                    channel.InitChannel();
                    channelMap.Add(channel.Name, channel);
                }
                else
                {
                    Debug.LogWarning($"Channel with name {channel.Name} already exist!");
                }
            }

            init = true;
        }


        public bool RemoveVariable(VariableBase variable, string channelName = "")
        {
            return TryGetChannel(channelName, out Channel channel) && channel.RemoveVariable(variable);
        }

        public bool RemoveGameEvent(GameEvent gameEvent,string channelName = "")
        {
            return TryGetChannel(channelName, out Channel channel) && channel.RemoveGameEvent(gameEvent);
        }

        public bool RemoveChannel(Channel channel)
        {
            if (channel != null
                && channels.Contains(channel) == false
                && channelMap.TryGetValue(channel.Name, out Channel source)
                && source == channel)
            {
                channel.DestroyNotPersistendObjects();
                var result = channel.IsChannelEmpty();

                if (result)
                {
                    channels.Remove(channel);
                    channelMap.Remove(channel.Name);
                }

                return result;
            }

            return false;
        }

        public bool TryGetVariableByName<T>(string name, out T variable,string channelName = "")
            where T : VariableBase
        {
            Init();

            variable = null;
            return TryGetChannel(channelName, out Channel channel) && channel.TryGetVariableByName<T>(name, out variable);
        }

        public bool TryGetGameEventByName<T>(string name, out T gameEvent,string channelName = "")
            where T : GameEventBase
        {
            Init();

            gameEvent = null;
            return TryGetChannel(channelName, out Channel channel) && channel.TryGetGameEventByName<T>(name,out gameEvent);
        }

        public bool TryGetChannel(string name, out Channel channel)
        {
            Init();

            return channelMap.TryGetValue(name,out channel);
        }


        public T GetOrCreateVariable<T>(string name,string channelName = "")
            where T : VariableBase
        {
            Init();

            return TryGetChannel(channelName, out Channel channel) ? channel.GetOrCreateVariable<T>(name) : defaultChannel.GetOrCreateVariable<T>(name);
        }

        public T GetOrCreateGameEvent<T>(string name,string channelName = "")
            where T : GameEventBase
        {
            Init();

            return TryGetChannel(channelName,out Channel channel) ? channel.GetOrCreateGameEvent<T>(name) : defaultChannel.GetOrCreateGameEvent<T>(name);
        }

        public Channel GetOrCreateChannel(string name)
        {
            Init();

            if (TryGetChannel(name,out Channel channel))
            {
                return channel;
            }
            else
            {
                channel = new Channel();
                channel.Name = name;
                channelMap.Add(channel.Name, channel);
                return channel;
            }
        }

        public bool ContaineVariable(string name)
        {
            Init();

            if (defaultChannel.ContaineVariable(name)) return true;

            foreach (var channel in channels)
            {
                if (channel.ContaineVariable(name)) return true;
            }

            return false;
        }

        public bool ContaineGameEvent(string name)
        {
            Init();

            if (defaultChannel.ContaineGameEvent(name)) return true;

            foreach (var channel in channels)
            {
                if (channel.ContaineGameEvent(name)) return true;
            }

            return false;
        }

        public bool ContaineChannel(Channel channel)
        {
            Init();

            return channelMap.ContainsKey(channel.Name);
        }
    }
}

