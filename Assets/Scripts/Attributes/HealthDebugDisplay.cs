using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDebugDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthValue;
        private Health _playerHealth;

        private void Start()
        {
            _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _healthValue.text = $"{_playerHealth.GetCurrentHealth()} / {_playerHealth.GetMaxHealth()}";
        }
    }
}
