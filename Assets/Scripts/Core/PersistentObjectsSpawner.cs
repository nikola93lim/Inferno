using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _persistentObjectPrefab;

        static bool hasSpawned;

        private void Awake()
        {
            if (hasSpawned) return;

            Application.targetFrameRate = 60;

            hasSpawned = true;

            SpawnPersistentObjects();
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
