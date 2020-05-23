using System;
using UnityEngine;

namespace GameConnection.UI
{
    public class EndMenu : MonoBehaviour
    {
        private Action _onRestartGameSequenceClicked;

        public void Init(Action onRestartGameSequenceClicked)
        {
            _onRestartGameSequenceClicked = onRestartGameSequenceClicked;
        }

        // Called from scene
        public void OnRestartSequenceClicked() => _onRestartGameSequenceClicked?.Invoke();
    }
}
