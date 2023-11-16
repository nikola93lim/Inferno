using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action OnXPGained;
        [SerializeField] private float _experiencePoints;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            OnXPGained?.Invoke();
        }

        public float GetXP()
        {
            return _experiencePoints;
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float) state;
        }
    }
}
