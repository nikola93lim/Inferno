using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.MainMenu
{
    public class SwitcherUI : MonoBehaviour
    {
        [SerializeField] private GameObject _entryPoint;

        private void Start()
        {
            SwitchTo(_entryPoint);
        }

        public void SwitchTo(GameObject toDisplay)
        {
            if (toDisplay.transform.parent != transform)
                return;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == toDisplay);
            }
        }
    }
}
