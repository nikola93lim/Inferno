using GameDevTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string _defaultSaveFile = "save";

        [SerializeField] private GameObject _tipsCanvas;
        [SerializeField] private GameObject _objectiveCanvas;
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadLastScene());
        }

        public void NewGame()
        {
            StartCoroutine(LoadFirstScene());
        }

        public void MainMenu()
        {
            StartCoroutine(LoadMainMenu());
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(0.5f);
            SetActive(true, _tipsCanvas);
            yield return SceneManager.LoadSceneAsync(1);
            SetActive(false, _tipsCanvas);
            yield return fader.FadeIn(0.5f);
            SetActive(true, _objectiveCanvas);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(0.5f);
            SetActive(true, _tipsCanvas);
            yield return _savingSystem.LoadLastScene(_defaultSaveFile);
            SetActive(false, _tipsCanvas);
            yield return fader.FadeIn(0.5f);
            SetActive(true, _objectiveCanvas);
        }

        private IEnumerator LoadMainMenu()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(0.5f);
            SetActive(true, _tipsCanvas);
            yield return SceneManager.LoadSceneAsync(0);
            SetActive(false, _tipsCanvas);
            yield return fader.FadeIn(0.5f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            _savingSystem.Save(_defaultSaveFile);
        }

        public void Load()
        {
            _savingSystem.Load(_defaultSaveFile);
        }

        public void Delete()
        {
            _savingSystem.Delete(_defaultSaveFile);
        }

        private void SetActive(bool isActive, GameObject go)
        {
            go.SetActive(isActive);
        }
    }
}
