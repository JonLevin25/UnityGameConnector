using System;
using TMPro;
using UnityEngine;

public class EnterCodeController : MonoBehaviour
{
    [SerializeField] private TMP_InputField passInput;
    [SerializeField] private TMP_Text display;


    private const char FillChar = '_';
    private int _passLength;
    private Action<string> _onCodeSubmitted;

    public void Init(string actualPass, Action<string> onCodeSubmitted)
    {
        _passLength = actualPass.Length;
        passInput.characterLimit = _passLength;
        
        _onCodeSubmitted = onCodeSubmitted;
        UpdateDisplayText();
    }
    
    // Called from scene
    public void OnTextValueChanged(string newValue)
    {
        UpdateDisplayText();
    }

    // Called from scene
    public void OnSubmitCode()
    {
        _onCodeSubmitted?.Invoke(passInput.text.ToUpper());
    }

    private void UpdateDisplayText()
    {
        var newValue = passInput.text.ToUpper();
        var padding = Mathf.Max(0, _passLength - newValue.Length);
        var str = $"{newValue}{new string(FillChar, padding)}";
        display.text = str;
    }
}
