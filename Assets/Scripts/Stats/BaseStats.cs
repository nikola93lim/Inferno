using GameDevTV.Utils;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action OnLeveledUp;

        [Range(1, 99)]
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progressionSO;
        [SerializeField] private GameObject _levelUpEffect;
        [SerializeField] private bool _isPlayer = false;
        private Experience _experience;

        private LazyValue<int> _currentLevel;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.OnXPGained += Experience_OnXPGained;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.OnXPGained -= Experience_OnXPGained;
            }
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        public CharacterClass GetCharacterClass()
        {
            return _characterClass;
        }
        private void Experience_OnXPGained()
        {
            int newLevel = CalculateLevel();
            if (_currentLevel.value != newLevel)
            {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                OnLeveledUp?.Invoke();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return GetBaseStat(stat) + GetAdditiveModifier(stat) + GetPercentageModifier(stat);
        }

        private float GetBaseStat(Stat stat)
        {
            return _progressionSO.GetStat(stat, _characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!_isPlayer) return 0f;

            float modifierAmount = 0f;

            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();

            foreach (IModifierProvider item in modifierProviders)
            {
                foreach (float modifier in item.GetAdditiveModifiers(stat))
                {
                    modifierAmount += modifier;
                }
            }

            return modifierAmount;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!_isPlayer) return 0f;

            float modifierAmount = 0f;

            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();

            foreach (IModifierProvider item in modifierProviders)
            {
                foreach (float modifier in item.GetPercentageModifiers(stat))
                {
                    modifierAmount += modifier;
                }
            }

            return modifierAmount;
        }

        public int GetLevel()
        {
            return _currentLevel.value;
        }

        public int CalculateLevel()
        {
            if (_experience == null) return _startingLevel;

            float currentXP = _experience.GetXP();

            int penultimateLevel = _progressionSO.GetLevel(_characterClass, Stat.ExperienceToLevelUp);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float experienceToLevelUp = _progressionSO.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if (experienceToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
