using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float _chaseDistance = 5f;
        [SerializeField] private float _alertNearbyRadius = 5f;
        [SerializeField] private float _suspicionStateTime = 3f;
        [SerializeField] private float _waypointDistanceTollerance = 0.2f;
        [SerializeField] private float _waypointDwellTime = 3f;
        [SerializeField] private float aggroCooldownTime = 5f;

        [Range(0f, 1f)]
        [SerializeField] private float _patrolStateSpeedFraction = 0.4f;
        [SerializeField] private PatrolPath _patrolPath;

        [SerializeField] private LayerMask _enemyLayerMask;

        private GameObject _player;
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;
        private ActionSystem _actionSystem;

        private LazyValue<Vector3> _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _currentWaypointDwellTime = Mathf.Infinity;
        private float _timeSinceLastAggrevated = Mathf.Infinity;
        private int _currentWaypointIndex;
        private bool _hasAlertedNearbyUnits = false;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionSystem = GetComponent<ActionSystem>();

            _guardPosition = new LazyValue<Vector3>(GetInitialGuardPosition);
            _player = GameObject.FindWithTag("Player");
        }

        private Vector3 GetInitialGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            _guardPosition.ForceInit();
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (IsAggrevated() && _fighter.CanAttack(_player))
            {
                AttackState();
            }
            else if (_timeSinceLastSawPlayer < _suspicionStateTime)
            {
                SuspicionState();
            }
            else
            {
                PatrolState();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            _timeSinceLastAggrevated = 0f;
        }

        private void AlertNearbyUnits()
        {
            if (_hasAlertedNearbyUnits) return;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _alertNearbyRadius, Vector3.up, 0f, _enemyLayerMask);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent(out AIController aIController))
                {
                    if (aIController == this) continue;
                    aIController.Aggrevate();
                }
            }
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _currentWaypointDwellTime += Time.deltaTime;
            _timeSinceLastAggrevated += Time.deltaTime;
        }

        private void AttackState()
        {
            _timeSinceLastSawPlayer = 0f;
            _fighter.Attack(_player.gameObject);

            AlertNearbyUnits();
            _hasAlertedNearbyUnits = true;
        }

        private void SuspicionState()
        {
            _actionSystem.CancelCurrentAction();
            _hasAlertedNearbyUnits = false;
        }

        private void PatrolState()
        {
            Vector3 nextPosition = _guardPosition.value;

            if (_patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _currentWaypointDwellTime = 0f;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_currentWaypointDwellTime > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, _patrolStateSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return UtilityClass.DistanceSquared(transform.position, GetCurrentWaypoint()) < _waypointDistanceTollerance;
        }

        private bool IsAggrevated()
        {
            float distanceToPlayerSq = UtilityClass.DistanceSquared(transform.position, _player.transform.position);
            return distanceToPlayerSq < _chaseDistance * _chaseDistance || _timeSinceLastAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}
