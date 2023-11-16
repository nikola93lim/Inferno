using GameDevTV.Inventories;
using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventory
{
    public class RandomItemDropper : ItemDropper
    {
        private const int ATTEMPTS = 30;
        [SerializeField] private float _scatterDistance = 1f;
        [SerializeField] private DropLibrarySO _dropLibrarySO;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            if (_health == null)
                return;

            _health.OnDied += _health_OnDied;
        }

        private void OnDisable()
        {
            if (_health == null)
                return;

            _health.OnDied -= _health_OnDied;
        }

        private void _health_OnDied(object sender, System.EventArgs e)
        {
            RandomDrop();
        }

        public void RandomDrop()
        {
            InventoryItem item = _dropLibrarySO.GetRandomDrop();

            if (item == null)
                return;

            DropItem(item, 1);
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 dropLocation = transform.position + Random.insideUnitSphere * _scatterDistance;

                if (NavMesh.SamplePosition(dropLocation, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            
            return transform.position;
        }
    }
}
