using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Animation
{
    public class CharacterAnimator : MonoBehaviour
    {
        private const string FORWARD_SPEED = "forwardSpeed";
        private const string ATTACK = "attack";
        private const string STOP_ATTACK = "stopAttack";
        private const string DIE = "die";
        private const string DRAW_SWORD = "drawSword";

        [SerializeField] private Animator _animator;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private WeaponConfigSO _weapon;

        private void OnEnable()
        {
            _fighter.OnAttack += _fighter_OnAttack;
            _fighter.OnAttackCancelled += _fighter_OnAttackCancelled;
            _health.OnDied += _health_OnDied;
        }

        private void OnDisable()
        {
            _fighter.OnAttack -= _fighter_OnAttack;
            _fighter.OnAttackCancelled -= _fighter_OnAttackCancelled;
            _health.OnDied -= _health_OnDied;
        }

        private void Update()
        {
            UpdateAnimator();
        }

        private void _fighter_OnAttack(object sender, System.EventArgs e)
        {
            _animator.ResetTrigger(STOP_ATTACK);
            _animator.SetTrigger(ATTACK);
        }

        private void _fighter_OnAttackCancelled(object sender, System.EventArgs e)
        {
            _animator.ResetTrigger(ATTACK);
            _animator.SetTrigger(STOP_ATTACK);
        }

        private void _health_OnDied(object sender, System.EventArgs e)
        {
            _animator.SetTrigger(DIE);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float forwardSpeed = localVelocity.z;

            _animator.SetFloat(FORWARD_SPEED, forwardSpeed);
        }

        // Animation event
        private void Hit()
        {
            _fighter.Hit();
        }

        // Animation event
        private void Shoot()
        {
            Hit();
        }

        // Used in main menu only to animate player drawing sword
        public void DrawSword()
        {
            _animator.SetTrigger(DRAW_SWORD);
        }

        // Animation event for drawing sword in main menu
        public void EquipWeapon()
        {
            _fighter.EquipWeapon(_weapon);
        }
    }
}
