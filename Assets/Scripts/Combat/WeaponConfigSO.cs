using GameDevTV.Inventories;
using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make New Weapon", order = 0)]
    public class WeaponConfigSO : EquipableItem
    {
        [SerializeField] Weapon _weaponPrefab = null;
        [SerializeField] AnimatorOverrideController _overrideController = null;
        [SerializeField] Projectile _projectilePrefab = null;

        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _weaponDamage = 20f;
        [SerializeField] private float _percentageBonus = 0f;
        [SerializeField] private bool _isRightHanded = true;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (_weaponPrefab != null)
            {
                Weapon weapon = Instantiate(_weaponPrefab, GetHandTransform(rightHand, leftHand));
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_overrideController != null)
            {
                animator.runtimeAnimatorController = _overrideController;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform leftHand, Transform rightHand)
        {
            Transform oldWeapon = leftHand.Find(weaponName);

            if (oldWeapon == null)
            {
                oldWeapon = rightHand.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(GameObject attacker, Transform rightHand, Transform leftHand, Health target, float calculatedDamage)
        {
            Projectile projectile = Instantiate(_projectilePrefab, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(attacker, target, calculatedDamage);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform hand;
            if (_isRightHanded)
            {
                hand = rightHand;
            }
            else
            {
                hand = leftHand;
            }

            return hand;
        }

        public float GetDamage()
        {
            return _weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return _percentageBonus;
        }

        public float GetRange()
        {
            return _weaponRange;
        }

        public bool HasProjectile()
        {
            return _projectilePrefab != null;
        }
    }
}
