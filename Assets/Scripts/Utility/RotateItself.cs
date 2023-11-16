using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utility
{
    public class RotateItself : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 15f;
        private void Update()
        {
            transform.eulerAngles += _rotateSpeed * Time.deltaTime * Vector3.up;
        }
    }
}
