using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Misc
{
    public class InteractableDoor : MonoBehaviour
    {
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;
        [SerializeField] private float _openSpeed = 5f;

        [SerializeField] private bool _shouldOpen = false;

        private void Update()
        {
            if (!_shouldOpen)
                return;

            if (!TryDisableScript())
            {
                _leftDoor.localEulerAngles -= Vector3.up * Time.deltaTime * _openSpeed;
                _rightDoor.localEulerAngles += Vector3.up * Time.deltaTime * _openSpeed;
            }
        }

        public void Interact()
        {
            _shouldOpen = true;
        }

        private bool TryDisableScript()
        {
            if (_leftDoor.localEulerAngles.y <= -90f || _rightDoor.localEulerAngles.y >= 90f)
            {
                this.enabled = false;
            }

            return false;
        }

    }
}
