using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float _waypointGizmoRadius = 0.3f;
        [SerializeField] private Transform[] _waypoints;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            for (int i = 0; i < _waypoints.Length; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), _waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            return i == _waypoints.Length - 1 ? 0 : i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return _waypoints[i].position;
        }
    }   
}
