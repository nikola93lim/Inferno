using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private void LateUpdate()
        {
            transform.position = _player.position;
        }
    }
}
