using RPG.Attributes;
using RPG.Control;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private GameObject _gameOverCanvas;
        [SerializeField] private SavingWrapper _savingWrapper;

        private Health _player;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Health>();
            _player.OnDied += _player_OnDied;

            _fader = FindObjectOfType<Fader>();
            _savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        private void _player_OnDied(object sender, System.EventArgs e)
        {
            DisplayGameOver();
        }

        private void DisplayGameOver()
        {
            StartCoroutine(GameOver());
        }

        private IEnumerator GameOver()
        {
            yield return _fader.FadeOut(1f);
            _gameOverCanvas.SetActive(true);
        }

        public void Restart()
        {
            _gameOverCanvas.SetActive(false);
            _savingWrapper.NewGame();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
