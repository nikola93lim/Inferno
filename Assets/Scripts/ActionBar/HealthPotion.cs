using GameDevTV.Inventories;
using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.ActionBar
{
    [CreateAssetMenu(menuName = "RPG/Consumables/Health Potion")]
    public class HealthPotion : ActionItem
    {
        [SerializeField] private float _healPercentage = 20f;

        public override void Use(GameObject user)
        {
            if (user.TryGetComponent(out Health health))
            {
                float healAmount = _healPercentage / health.GetMaxHealth() * health.GetMaxHealth();
                health.Heal(healAmount);
            }
        }
    }
}
