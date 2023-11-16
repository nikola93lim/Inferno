using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject _hitEffect = null;
        [SerializeField] private ParticleSystem _trail = null;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private bool _isHoming = false;
        private Health _target = null;
        private float _damage = 0f;
        private GameObject _attacker;

        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        private void Update()
        {
            if (_target == null) return;

            if (_isHoming) transform.LookAt(GetAimPosition());

            transform.Translate(_speed * Time.deltaTime * Vector3.forward);
        }

        public void SetTarget(GameObject attacker, Health target, float damage)
        {
            _target = target;
            _damage = damage;
            _attacker = attacker;
            transform.LookAt(GetAimPosition());
        }

        private Vector3 GetAimPosition()
        {
            CapsuleCollider collider = _target.GetComponent<CapsuleCollider>();
            if (collider == null) return _target.transform.position;

            return _target.transform.position + Vector3.up * collider.height / 2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Health>(out Health target))
            {
                if (target != _target) return;
                target.TakeDamage(_attacker, _damage);

                if (_trail != null)
                {
                    _trail.transform.parent = null;
                    Destroy(_trail.gameObject, 1f);
                }

                if (_hitEffect != null)
                {
                    GameObject hit = Instantiate(_hitEffect, GetAimPosition(), transform.rotation);
                    Destroy(hit, 1f);
                }

                Destroy(gameObject);
            }
        }
    }
}
