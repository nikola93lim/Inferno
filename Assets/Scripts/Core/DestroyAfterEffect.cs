using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_particleSystem.IsAlive()) return;

            if (_parent != null)
            {
                Destroy(_parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
