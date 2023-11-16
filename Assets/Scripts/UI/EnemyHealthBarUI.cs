using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class EnemyHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private BaseStats _parent;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private float _fadeTime = 0.5f;

        private Coroutine _fadeCoroutine;

        private void Start()
        {
            SetNameLabel(_parent.GetCharacterClass().ToString());
            SetLevelText(_parent.GetLevel());
            ShowBar(false);
        }

        // hooked to Unity Event on Health component
        public void UpdateHealthBar(float reduceAmountNormalized)
        {
            ShowBar(true);

            _bar.fillAmount -= reduceAmountNormalized;

            if(_bar.fillAmount <= 0f) Destroy(gameObject);
        }

        public void ShowBar(bool show)
        {
            StartCoroutine(ShowBarCoroutine(show));
        }

        public IEnumerator ShowBarCoroutine(bool show)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            if (show)
            {
                _fadeCoroutine = StartCoroutine(FadeInHealthBar());
                yield return _fadeCoroutine;
            }
            else
            {
                _fadeCoroutine = StartCoroutine(FadeOutHealthBar());
                yield return _fadeCoroutine;
            }
        }

        private IEnumerator FadeInHealthBar()
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime / _fadeTime;
                yield return null;
            }
        }

        private IEnumerator FadeOutHealthBar()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime / _fadeTime;
                yield return null;
            }
        }

        public void SetNameLabel(string name)
        {
            _nameLabel.text = name;
        }

        public void SetLevelText(int level)
        {
            _level.text = level.ToString();
        }
    }
}
