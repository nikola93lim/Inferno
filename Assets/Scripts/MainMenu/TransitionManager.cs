using Cinemachine;
using RPG.Animation;
using RPG.Combat;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.MainMenu
{
    public partial class TransitionManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Fighter _fighter;

        public void Transition(Button button)
        {
            StartCoroutine(TransitionCoroutine(button));
        }

        private IEnumerator TransitionCoroutine(Button button)
        {
            _virtualCamera.Priority = 100;

            while (_canvasGroup.alpha > 0f)
            {
                _characterAnimator.transform.forward = Vector3.Lerp(_characterAnimator.transform.forward, -_virtualCamera.transform.forward, Time.deltaTime * 2f);
                _canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }

            _characterAnimator.DrawSword();

            yield return new WaitForSeconds(2f);

            if (button == Button.Continue)
            {
                FindObjectOfType<SavingWrapper>().ContinueGame();
            }
            else if (button == Button.NewGame)
            {
                FindObjectOfType<SavingWrapper>().NewGame();
            }
        }
    }
}
