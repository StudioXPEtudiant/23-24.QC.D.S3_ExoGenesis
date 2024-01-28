using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Characters.AI.Senses
{
    public abstract class Sense : MonoBehaviour
    {
        [SerializeField] private float memoryDuration = 5;
        [SerializeField] private string[] categories;
        [SerializeField] private UnityEvent<AITarget> action;
        [SerializeField] private UnityEvent<AITarget> actionStop;

        public UnityEvent<AITarget> Action => action;
        public UnityEvent<AITarget> ActionStop => actionStop;

        private readonly Dictionary<AITarget, float> _memory = new();
        private readonly Dictionary<string, int> _filter = new();

        private void Awake()
        {
            for(int i = 0; i < categories.Length; i++)
                _filter.Add(categories[i], i);
        }

        protected abstract bool OnIsSensing(AITarget target);

        public bool IsSensing(AITarget target)
        {
            var isSensing = _filter.ContainsKey(target.Category) && OnIsSensing(target);

            if (!isSensing)
            {
                if (!_memory.ContainsKey(target))
                    return false;

                if (Time.time - _memory[target] <= memoryDuration) return true;
                
                _memory.Remove(target);
                return false;
            }
            
            if (!_memory.ContainsKey(target))
                _memory.Add(target, Time.time);
            else
                _memory[target] = Time.time;

            return true;
        }

        public List<AITarget> GetSensedTargets()
        {
            return AITarget.Targets.Where(target => target.gameObject.activeInHierarchy).Where(IsSensing).ToList();
        }
        
        public AITarget GetClosestSensedTarget()
        {
            AITarget closestTarget = null;
            float closestDistance = 0;
            float currentPriority = 0;

            foreach (var target in GetSensedTargets())
            {
                if (!_filter.ContainsKey(target.Category)) continue;
                
                var distance = Vector3.Distance(transform.position, target.transform.position);
                var priority = _filter[target.Category];
                if (   !ReferenceEquals(closestTarget, null) 
                    && distance > closestDistance
                    && priority > currentPriority) continue;
                
                closestTarget = target;
                closestDistance = distance;
                currentPriority = priority;
            }

            return closestTarget;
        }
    }
}
