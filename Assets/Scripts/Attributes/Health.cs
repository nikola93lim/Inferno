using GameDevTV.Utils;
using RPG.Animation;
using GameDevTV.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public UnityEvent OnDiedUnityEvent; // quick fix, needs refactoring to only one OnDead event
        public event Action OnHealthValueChanged;
        public event EventHandler OnDied;
        public event EventHandler OnHit;
        public OnTakenDamageEvent OnTakenDamage;
        public OnTakenDamageNormalizedEvent OnTakenDamageNormalized;

        [Serializable]
        public class OnTakenDamageEvent : UnityEvent<float>
        {
        }

        [Serializable]
        public class OnTakenDamageNormalizedEvent : UnityEvent<float>
        {
        }

        [SerializeField] private float _regenerationPercentage = 70f;

        private LazyValue<float> _currentHealth;

        private Animator _animator;
        private BaseStats _baseStats;

        private bool _isDead = false;

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _animator = GetComponentInChildren<Animator>();

            _currentHealth = new LazyValue<float>(GetInitialHealth);
        }

        private void OnEnable()
        {
            _baseStats.OnLeveledUp += _baseStats_OnLeveledUp;
        }

        private void OnDisable()
        {
            _baseStats.OnLeveledUp -= _baseStats_OnLeveledUp;
        }

        private void Start()
        {
            _currentHealth.ForceInit();
        }

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        private void _baseStats_OnLeveledUp()
        {
            RegenerateHealth();
        }

        private void RegenerateHealth()
        {
            float regenHealth = _baseStats.GetStat(Stat.Health) * (_regenerationPercentage / 100f);
            _currentHealth.value = MathF.Max(_currentHealth.value, regenHealth);

            OnHealthValueChanged?.Invoke();
        }

        public void Heal(float amount)
        {
            _currentHealth.value += amount;
            if (_currentHealth.value > GetMaxHealth())
                _currentHealth.value = GetMaxHealth();

            OnHealthValueChanged?.Invoke();
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject attacker, float damage)
        {
            _currentHealth.value = Mathf.Max(_currentHealth.value - damage, 0f);

            if (_currentHealth.value == 0)
            {
                Die();
                AwardExperience(attacker);
            }

            OnTakenDamage?.Invoke(damage);

            float maxHealth = _baseStats.GetStat(Stat.Health);
            OnTakenDamageNormalized?.Invoke(damage / maxHealth);

            OnHit?.Invoke(this, EventArgs.Empty);

            OnHealthValueChanged?.Invoke();
        }

        private void AwardExperience(GameObject attacker)
        {
            if (attacker.TryGetComponent(out Experience experience))
            {
                experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
            }
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            GetComponent<Collider>().enabled = false;

            OnDied?.Invoke(this, EventArgs.Empty);
            _animator.SetTrigger("die");

            OnDiedUnityEvent?.Invoke();
        }

        public float GetCurrentHealth()
        {
            return _currentHealth.value;
        }

        public float GetMaxHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return _currentHealth.value;
        }

        public void RestoreState(object state)
        {
            float health = (float) state;

            _currentHealth.value = health;

            if (_currentHealth.value == 0)
            {
                Die();
            }
        }
    }
}
