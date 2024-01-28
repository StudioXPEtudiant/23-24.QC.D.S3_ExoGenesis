using StudioXP.Scripts.Events;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Characters
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private FloatEvent healthChanged;
        [SerializeField] private FloatEvent healthDecreased;
        [SerializeField] private FloatEvent healthIncreased;
        [SerializeField] private UnityEvent healthReachedMin;
        [SerializeField] private UnityEvent healthReachedMax;
        [SerializeField] private FloatEvent maxHealthChanged;

        private int _health;

        public int MaxValue
        {
            get => maxHealth;
            set
            {
                maxHealth = value < 1 ? 1 : value;
                if (Value > maxHealth)
                    Value = maxHealth;
                
                maxHealthChanged.Invoke(maxHealth);
            }
        }

        public int Value
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
            
                if (value < 0)
                    _health = 0;
                else if (value > maxHealth)
                    _health = maxHealth;
                else
                    _health = value;
            
                InvokeEvents();
            }
        }

        public FloatEvent HealthChanged => healthChanged;
        public FloatEvent HealthDecreased => healthDecreased;
        public FloatEvent HealthIncreased => healthIncreased;
        public UnityEvent HealthReachedMin => healthReachedMin;
        public UnityEvent HealthReachedMax => healthReachedMax;

        private void Start()
        {
            _health = maxHealth;
            maxHealthChanged.Invoke(maxHealth);
            InvokeEvents();
        }

        public void Increase(int health)
        {
            if (_health == maxHealth || health <= 0)
                return;
            
            _health += health;
            
            healthIncreased.Invoke(_health);
            if (_health > maxHealth)
                _health = maxHealth;
            
            InvokeEvents();
        }

        public void Decrease(int health)
        {
            if (_health == 0 || health <= 0)
                return;
            
            _health -= health;
            
            healthDecreased.Invoke(_health);
            if (_health < 0)
                _health = 0;
            
            InvokeEvents();
        }

        private void InvokeEvents()
        {
            healthChanged.Invoke(_health);
            
            if(_health == 0)
                healthReachedMin.Invoke();
            else if(_health == maxHealth)
                healthReachedMax.Invoke();
        }

        public void RefillHealthToMax()
        {
            _health = maxHealth;
        }
    }
}
