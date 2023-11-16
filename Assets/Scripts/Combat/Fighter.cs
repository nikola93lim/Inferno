using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Stats;
using RPG.Attributes;
using GameDevTV.Utils;
using GameDevTV.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        public event EventHandler OnAttack;
        public event EventHandler OnAttackCancelled;

        [SerializeField] private WeaponConfigSO _defaultWeaponSO;
        [SerializeField] private LazyValue<WeaponConfigSO> _currentWeaponSO;
        [SerializeField] private Transform _rightHandTransform;
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private float _timeBetweenAttacks = 1f;

        [Range(0f, 1f)]
        [SerializeField] private float _enemyAttackStateMoveSpeedFraction = 0.85f;
        private float speedFractionValueToSet;


        // current target info
        private Health _target;

        // components
        private Mover _mover;
        private ActionSystem _actionSystem;
        private BaseStats _baseStats;
        private Equipment _equipment;

        // misc
        private float _timeSinceLastAttack;

        private void Awake()
        {
            _equipment = GetComponent<Equipment>();
            _baseStats = GetComponent<BaseStats>();
            _mover = GetComponent<Mover>();
            _actionSystem = GetComponent<ActionSystem>();

            if (gameObject.CompareTag("Player"))
            {
                speedFractionValueToSet = 1f;
            }
            else
            {
                speedFractionValueToSet = _enemyAttackStateMoveSpeedFraction;
            }

            _currentWeaponSO = new LazyValue<WeaponConfigSO>(SetupDefaultWeapon);

            if (_equipment)
                _equipment.equipmentUpdated += UpdateWeapon;
        }

        private void Start()
        {
            _currentWeaponSO.ForceInit();
        }

        private void UpdateWeapon()
        {
            var weapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfigSO;

            if (weapon != null)
            {
                EquipWeapon(weapon);
            }
            else
            {
                EquipWeapon(_defaultWeaponSO);
            }
        }

        private WeaponConfigSO SetupDefaultWeapon()
        {
            AttachWeapon(_defaultWeaponSO);
            return _defaultWeaponSO;
        }

        public void EquipWeapon(WeaponConfigSO weapon)
        {
            _currentWeaponSO.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(WeaponConfigSO weapon)
        {
            Animator animator = GetComponentInChildren<Animator>();
            weapon.Spawn(_rightHandTransform, _leftHandTransform, animator);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.IsDead()) return;

            if (!IsInRange(_target.transform))
            {
                _mover.MoveTo(_target.transform.position, speedFractionValueToSet);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            Vector3 dirToTarget = (_target.transform.position - transform.position).normalized;
            float rotationSpeed = 30f;
            transform.forward = Vector3.Lerp(transform.forward, dirToTarget, Time.deltaTime * rotationSpeed);

            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                _timeSinceLastAttack = 0f;
            }
        }

        // Animation event
        public void Hit()
        {
            if (_target == null) return;

            float damage = _baseStats.GetStat(Stat.Damage);

            if (_currentWeaponSO.value.HasProjectile())
            {
                _currentWeaponSO.value.LaunchProjectile(gameObject, _rightHandTransform, _leftHandTransform, _target, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }

        private bool IsInRange(Transform target)
        {
            float weaponRange = _currentWeaponSO.value.GetRange();
            return UtilityClass.DistanceSquared(transform.position, target.position) < weaponRange * weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!_mover.CanMoveTo(combatTarget.transform.position) && !IsInRange(combatTarget.transform)) return false;

            Health health = combatTarget.GetComponent<Health>();

            return health != null && !health.IsDead() ? true : false;
        }

        public void Attack(GameObject combatTarget)
        {
            _actionSystem.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            OnAttackCancelled?.Invoke(this, EventArgs.Empty);
            _mover.Cancel();
            _target = null;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponSO.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponSO.value.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return _currentWeaponSO.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;

            WeaponConfigSO weapon = Resources.Load<WeaponConfigSO>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
