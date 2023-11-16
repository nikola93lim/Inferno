using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionSystem : MonoBehaviour
    {
        private IAction _action;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnDied += _health_OnDied;
        }

        private void OnDisable()
        {
            _health.OnDied -= _health_OnDied;
        }

        private void _health_OnDied(object sender, System.EventArgs e)
        {
            CancelCurrentAction();
        }

        public void StartAction(IAction action)
        {
            if (_action == action) return;

            if (_action != null)
            {
                _action.Cancel();
            }

            _action = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
