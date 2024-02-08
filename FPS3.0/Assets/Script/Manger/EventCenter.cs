using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    public class EventCenter : Singleton<EventCenter>
    {
        public delegate void ProcessEventDelegate(object obj, int param1, int param2);

        public Dictionary<string, ProcessEventDelegate> eventMap = new Dictionary<string, ProcessEventDelegate>();

        public void Register(string name, ProcessEventDelegate listener)
        {
            if (eventMap.ContainsKey(name))
            {
                eventMap[name] += listener;
            }
            else
            {
                eventMap.Add(name, listener);
            }
        }

        public void Unregister(string name, ProcessEventDelegate listener)
        {
            if (eventMap.ContainsKey(name))
            {
                eventMap.Remove(name);
            }
        }

        public void Trigger(string name, object obj, int param1, int param2)
        {            
            if (eventMap.ContainsKey(name))
            {
                eventMap[name].Invoke(obj, param1, param2);
            }
        }
    }
}