using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        private const float MIN_ZOOM_DISTANCE = 2f;
        private const float MAX_ZOOM_DISTANCE = 15f;

        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private CinemachineFramingTransposer _cinemachineFramingTransposer;
        private Vector3 targetFollowOffset;

        private void Start()
        {
            _cinemachineFramingTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update()
        {
            HandleRotation();
            //HandleZoom();
        }

        private void HandleZoom()
        {
            float zoomAmount = 1f;

            if (Input.mouseScrollDelta.y > 0)
            {
                targetFollowOffset.y -= zoomAmount;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                targetFollowOffset.y += zoomAmount;
            }

            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_ZOOM_DISTANCE, MAX_ZOOM_DISTANCE);

            float zoomSpeed = 5f;
            _cinemachineFramingTransposer.m_CameraDistance = Mathf.Lerp(_cinemachineFramingTransposer.m_CameraDistance, targetFollowOffset.y, Time.deltaTime * zoomSpeed);
        }

        private void HandleRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = 1f;
            }

            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = -1f;
            }

            float rotationSpeed = 100f;

            _cinemachineVirtualCamera.transform.eulerAngles += rotationSpeed * Time.deltaTime * rotationVector;
        }
    }
}