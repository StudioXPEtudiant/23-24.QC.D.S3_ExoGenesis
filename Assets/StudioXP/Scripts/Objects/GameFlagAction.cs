using System;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using StudioXP.Scripts.Game;
using UnityEngine;

namespace StudioXP.Scripts.Objects
{
    public class GameFlagAction : MonoBehaviour
    {
        [SerializeField] private GameFlag defaultFlag;
        
        public void TriggerDefault()
        {
            GameFlagCollection.Instance.Trigger(defaultFlag);
        }

        public void ClearDefault()
        {
            GameFlagCollection.Instance.Clear(defaultFlag);
        }
        
        //Format context:name:count
        public void TriggerCustom(string expression)
        {
           GameFlagCollection.Instance.Trigger(Parse(expression));
        }

        public void ClearCustom(string expression)
        {
            GameFlagCollection.Instance.Clear(Parse(expression));
        }

        private GameFlag Parse(string expression)
        {
            var match = Regex.Match(expression, @"(\w+):(\w+)(?::(\d+))?");
           
            GameFlag flag;
            flag.context = match.Groups[1].Value;
            flag.name = match.Groups[2].Value;
            flag.count = match.Groups[3].Success ? Convert.ToInt32(match.Groups[3].Value) : 0;

            return flag;
        }
    }
}
