using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private TransitionManager _transitionManager;

        public void Continue()
        {
            _transitionManager.Transition(Button.Continue);
        }

        public void NewGame()
        {
            _transitionManager.Transition(Button.NewGame);
        }

        public void Load()
        {
            _transitionManager.Transition(Button.Load);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
