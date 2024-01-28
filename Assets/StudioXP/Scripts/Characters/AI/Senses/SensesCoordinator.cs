using System;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Characters.AI.Senses
{
    public class SensesCoordinator : MonoBehaviour
    {
        [SerializeField] private float reactionTime = 0.05f;
        [SerializeField] private Sense[] senses;
        [SerializeField] private UnityEvent defaultAction;
        [SerializeField] private UnityEvent defaultActionStop;

        private float _reactionTimer;
        private AITarget _lastTarget;
        private Sense _lastSense;
        private bool isStunned = false;

        private void Start()
        {
            _reactionTimer = reactionTime;
        }

        private void Update()
        {
            if(isStunned) return;
            _reactionTimer -= Time.deltaTime;
            if (!(_reactionTimer <= 0)) return;
            
            _reactionTimer += reactionTime;

            AITarget currentTarget = null;
            Sense currentSense = null;
            foreach (var sense in senses)
            {
                currentSense = sense;
                currentTarget = sense.GetClosestSensedTarget();
                if (!currentTarget) continue;
                sense.Action.Invoke(currentTarget);
                break;
            }

            if (!currentTarget)
                currentSense = null;

            if (_lastTarget != currentTarget || _lastSense != currentSense)
            {
                if (_lastSense)
                    _lastSense.ActionStop.Invoke(_lastTarget);
                else if(_lastSense != currentSense)
                    defaultActionStop.Invoke();
                
                _lastTarget = currentTarget;
                _lastSense = _lastTarget ? currentSense : null;
            }
            
            if(!_lastTarget)
                defaultAction.Invoke();
        }

        public void SetIsStunnedTrue()
        {
            isStunned = true;
        }

        public void SetIsStunnedFalse()
        {
            isStunned = false;
        }
    }
}
