using System;
using UnityEngine;

namespace StudioXP.Scripts.Characters.AI
{
    [RequireComponent(typeof(Animator))]
    public class AIAttackAction : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private new string animation;
        
        private Animator _animator;
        private int _attackingAnimationId;
        private AITarget _target;

        public void Attack(AITarget target)
        {
            _target = target;
            _animator.SetBool(_attackingAnimationId, true);
        }

        public void Stop(AITarget target)
        {
            _target = null;
            _animator.SetBool(_attackingAnimationId, false);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _attackingAnimationId = Animator.StringToHash(animation);
        }

        public void DealDamage()
        {
            if (_target == null) return;
            _target.Health.Decrease(damage);
            
            if (_animator && _target.Health.Value <= 0)
                _animator.SetBool(_attackingAnimationId, false);
        }
    }
}
