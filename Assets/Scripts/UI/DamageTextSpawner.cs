using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageTextUI _damageTextPrefab;

        // hooked to Unity Event on Health component
        public void Spawn(float damageTextAmount)
        {
            DamageTextUI instance = Instantiate(_damageTextPrefab, transform);
            instance.SetDamageText(damageTextAmount);
        }
    }
}
