using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDebugDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _xpValue;
        private Experience _playerExperience;

        private void Start()
        {
            _playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _xpValue.text = _playerExperience.GetXP().ToString();
        }
    }
}
