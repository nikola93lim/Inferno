using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Drop Library")]
    public class DropLibrarySO : ScriptableObject
    {
        [SerializeField] private DropConfig[] _potentialDrops;
        [SerializeField] private float _dropChancePercentage;

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float relativeDropChance;
        }

        public InventoryItem GetRandomDrop()
        {
            if (!ShouldDropItem())
                return null;

            DropConfig dropConfig = ChooseRandomItem();
            return dropConfig.item;
        }

        private DropConfig ChooseRandomItem()
        {
            float totalChance = GetTotalChance();
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0f;

            foreach (var item in _potentialDrops)
            {
                chanceTotal += item.relativeDropChance;

                if (randomRoll <= chanceTotal)
                    return item;
            }

            return null;
        }

        private float GetTotalChance()
        {
            float totalChance = 0f;

            foreach (var item in _potentialDrops)
            {
                totalChance += item.relativeDropChance;
            }

            return totalChance;
        }

        private bool ShouldDropItem()
        {
            return Random.Range(0f, 100f) <= _dropChancePercentage;
        }
    }
}
