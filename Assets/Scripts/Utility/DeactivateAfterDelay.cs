using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utility
{
    public class DeactivateAfterDelay : MonoBehaviour
    {
        [SerializeField] private float _delayTime = 3f;

        private float _elapsedTime = 0f;

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _delayTime)
            {
                _elapsedTime = 0f;
                gameObject.SetActive(false);
            }
        }
    }
}
