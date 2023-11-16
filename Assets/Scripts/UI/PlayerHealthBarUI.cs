using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class PlayerHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        private Health _playerHealth;

        private void Start()
        {
            _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            _playerHealth.OnHealthValueChanged += _playerHealth_OnHealthValueChanged;

            UpdateFillAmount();
        }

        private void Update()
        {
            UpdateFillAmount();
        }

        private void _playerHealth_OnHealthValueChanged()
        {
            UpdateFillAmount();
        }

        private void UpdateFillAmount()
        {
            _fillImage.fillAmount = _playerHealth.GetCurrentHealth() / _playerHealth.GetMaxHealth();
        }
    }
}
