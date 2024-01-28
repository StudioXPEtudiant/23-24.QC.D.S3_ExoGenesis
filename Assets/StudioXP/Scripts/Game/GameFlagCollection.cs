using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudioXP.Scripts.Game
{
    public class GameFlagCollection : MonoBehaviour
    {
        public static GameFlagCollection Instance { get; private set; }

        private readonly Dictionary<string, Dictionary<string, int>> _flags = new();
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public bool IsTriggered(GameFlag flag)
        {
            if (!IsValid(flag)) return false;
            flag = Normalize(flag);
            
            return _flags.ContainsKey(flag.context) && _flags[flag.context].ContainsKey(flag.name) && _flags[flag.context][flag.name] >= flag.count;
        }

        public void Trigger(GameFlag flag)
        {
            if (!IsValid(flag)) return;
            flag = Normalize(flag);
            
            if (!_flags.ContainsKey(flag.context))
                _flags.Add(flag.context, new Dictionary<string, int>());

            if (!_flags[flag.context].ContainsKey(flag.name))
                _flags[flag.context].Add(flag.name, 1);
            else
                _flags[flag.context][flag.name] += flag.count;
        }

        public void Clear(GameFlag flag)
        {
            if (!IsValid(flag)) return;
            flag = Normalize(flag);
            
            if (!_flags.ContainsKey(flag.context) || 
                !_flags[flag.context].ContainsKey(flag.name)) return;

            _flags[flag.context].Remove(flag.name);
        }

        private static GameFlag Normalize(GameFlag flag)
        {
            if (string.IsNullOrEmpty(flag.context))
                flag.context = "global";

            return flag;
        }

        private static bool IsValid(GameFlag flag)
        {
            return !string.IsNullOrEmpty(flag.name);
        }
    }
}
