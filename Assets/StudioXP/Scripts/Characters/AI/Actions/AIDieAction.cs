using UnityEngine;

namespace StudioXP.Scripts.Characters.AI.Actions
{
    public class AIDieAction : MonoBehaviour
    {
        [SerializeField] private string deathAnimation;
        [SerializeField] private string deathCategory;
        [SerializeField] private int corpseHealth = 5;
        [SerializeField] private AITarget aiTarget;
        [SerializeField] private Health health;
        [SerializeField] private Behaviour[] behavioursToDisable;
        [SerializeField] private string[] animationsToDisable;

        private Animator _animator;
        private int _deathAnimationHash;
        private bool _firstDestroyCallIgnored = false;
        private int[] _animationsToDisableHash;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _deathAnimationHash = Animator.StringToHash(deathAnimation);

            _animationsToDisableHash = new int[animationsToDisable.Length];
            for (var i = 0; i < animationsToDisable.Length; i++)
                _animationsToDisableHash[i] = Animator.StringToHash(animationsToDisable[i]);

            if (!aiTarget)
                aiTarget = GetComponent<AITarget>();

            if (!health)
                health = GetComponent<Health>();
        }

        public void Die()
        {
            foreach (var animationToDisable in _animationsToDisableHash)
                _animator.SetBool(animationToDisable, false);
            
            _animator.SetBool(_deathAnimationHash, true);
            
            foreach (var behaviour in behavioursToDisable)
                behaviour.enabled = false;

            aiTarget.Category = deathCategory;
            
            health.HealthReachedMin.RemoveAllListeners();
            health.MaxValue = corpseHealth;
            health.Value = corpseHealth;
            health.HealthReachedMin.AddListener(Destroy);
        }

        private void Destroy()
        {
            if (_firstDestroyCallIgnored)
                Destroy(gameObject);
            else
                _firstDestroyCallIgnored = true;
        }
    }
}
