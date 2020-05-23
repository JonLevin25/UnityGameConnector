using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NamesSelectionGame.Runtime
{
    public class NamePrompt : UIBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        public event Action<string> OnSubmit;

        // Called from scene
        public void OnSubmitClicked()
        {
            OnSubmit?.Invoke(inputField.text);
        }
    }
}