using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] _cursorMappingArray;
        [SerializeField] private float _maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float _sphereCastRadius = 1f;
        [SerializeField] private LayerMask _navMeshLayerMask;

        private ActionStore _actionStore;
        private Mover _mover;
        private Health _health;
        private CombatTarget _currentCombatTarget;

        private bool _isDraggingUI = false;

        private void Awake()
        {
            _actionStore = GetComponent<ActionStore>();
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            CheckForActionBarUse();

            if (InteractWithUI()) return;

            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponents()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private void CheckForActionBarUse()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _actionStore.Use(0, gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _actionStore.Use(1, gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _actionStore.Use(2, gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _actionStore.Use(3, gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _actionStore.Use(4, gameObject);
            }
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
                _isDraggingUI = false;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                    _isDraggingUI = true;

                SetCursor(CursorType.UI);
                return true;
            }

            if (_isDraggingUI)
                return true;

            return false;
        }

        private bool InteractWithComponents()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.TryHandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        if (raycastable is CombatTarget)
                        {
                            _currentCombatTarget = raycastable as CombatTarget;
                            _currentCombatTarget.GetComponentInChildren<EnemyHealthBarUI>().ShowBar(true);
                        }
                        return true;
                    }
                }
            }

            if (_currentCombatTarget == null) return false;

            EnemyHealthBarUI healthBarUI = _currentCombatTarget.GetComponentInChildren<EnemyHealthBarUI>();
            if (healthBarUI != null)
            {
                healthBarUI.ShowBar(false);
            }

            _currentCombatTarget = null;

            return false;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = RaycastNavmesh(out Vector3 target);

            if (hasHit)
            {
                if (!_mover.CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit, 100f, _navMeshLayerMask);
            if (!hasHit) return false;

            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), _sphereCastRadius);
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping item in _cursorMappingArray)
            {
                if (item.type != type) continue;
                return item;
            }

            return new CursorMapping();
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
