using RPG.Control;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        private PlayerController _playerController;
        private SavingWrapper _savingWrapper;

        private void OnEnable()
        {
            Time.timeScale = 0f;
            _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _savingWrapper = FindObjectOfType<SavingWrapper>();
            _playerController.enabled = false;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            _playerController.enabled = true;
        }

        public void NewGame()
        {
            Time.timeScale = 1f;
            _savingWrapper.NewGame();
        }

        public void Quit()
        {
            Time.timeScale = 1f;
            _savingWrapper.MainMenu();
        }
    }
}
