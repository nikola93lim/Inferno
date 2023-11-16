using Cinemachine;
using RPG.Attributes;
using RPG.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Moments
{
    public class CenterHallDoorUnlock : MonoBehaviour
    {
        [SerializeField] private List<Health> _enemyList;
        [SerializeField] private InteractableDoor _door;
        [SerializeField] private CinemachineVirtualCamera _camera;

        private void Start()
        {
            foreach (Health enemy in _enemyList)
            {
                enemy.OnDied += Enemy_OnDied;
            }
        }

        private void Enemy_OnDied(object sender, System.EventArgs e)
        {
            _enemyList.Remove(sender as Health);

            if (_enemyList.Count == 0)
                TriggerUnlock();
        }

        private void TriggerUnlock()
        {
            _camera.Priority = 100;
            _door.Interact();
            Invoke(nameof(ResetCamera), 4f);
        }

        private void ResetCamera()
        {
            _camera.Priority = 0;
        }
    }
}
