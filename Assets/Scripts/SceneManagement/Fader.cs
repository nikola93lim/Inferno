using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutInstantly()
        {
            _canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut(float time)
        {
            if (_fadeCoroutine != null )
            {
                StopCoroutine(_fadeCoroutine);
            }

            _fadeCoroutine = StartCoroutine(FadeOutCoroutine(time));
            yield return _fadeCoroutine;
        }

        public IEnumerator FadeIn(float time)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _fadeCoroutine = StartCoroutine(FadeInCoroutine(time));
            yield return _fadeCoroutine;
        }

        private IEnumerator FadeOutCoroutine(float time)
        {
            while (_canvasGroup.alpha < 1.0f)
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        private IEnumerator FadeInCoroutine(float time)
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
