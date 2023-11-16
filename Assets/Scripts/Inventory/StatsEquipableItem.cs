using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Stats Equipable Item", menuName = "RPG/Inventory/Equipable Item")]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [System.Serializable]
        public struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public Modifier[] additiveModifiers;
        public Modifier[] percentageModifiers;

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat != stat)
                    continue;

                yield return modifier.value;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat != stat)
                    continue;

                yield return modifier.value;
            }
        }
    }
}
