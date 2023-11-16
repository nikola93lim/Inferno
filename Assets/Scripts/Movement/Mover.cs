using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _maxPathLength = 40f;

        private NavMeshAgent _agent;
        private ActionSystem _actionSystem;
        private Health _health;
        private float _maxSpeed;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _actionSystem = GetComponent<ActionSystem>();
            _health = GetComponent<Health>();
            _maxSpeed = _agent.speed;
        }

        private void Update()
        {
            _agent.enabled = !_health.IsDead();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionSystem.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _agent.SetDestination(destination);
            _agent.speed = _maxSpeed * speedFraction;
            _agent.isStopped = false;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > _maxPathLength) return false;

            return true;
        }

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return _agent;
        }

        private float GetPathLength(NavMeshPath path)
        {
            if (path.corners.Length < 2) return 0f;

            float totalDistanceSquared = 0f;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                totalDistanceSquared += UtilityClass.DistanceSquared(path.corners[i], path.corners[i + 1]);
            }

            return Mathf.Sqrt(totalDistanceSquared);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>) state;

            _agent.enabled = false;
            transform.position = ((SerializableVector3) data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3) data["rotation"]).ToVector();
            _agent.enabled = true;
        }
    }
}
