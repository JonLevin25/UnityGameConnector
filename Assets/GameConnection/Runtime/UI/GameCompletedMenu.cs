using System;
using UnityEngine;

namespace GameConnection.UI
{
    public class GameCompletedMenu : MonoBehaviour
    {
        private Action _onNextGameClicked;
        private Action _onRestartGameClicked;

        public void Init(Action onRestartGameClicked, Action onNextGameClicked)
        {
            _onNextGameClicked = onNextGameClicked;
            _onRestartGameClicked = onRestartGameClicked;
        }

        // Called from scene
        public void OnRestartGameClicked() => _onRestartGameClicked?.Invoke();

        // Called from scene
        public void OnNextGameClicked() => _onNextGameClicked?.Invoke();
    }
}
