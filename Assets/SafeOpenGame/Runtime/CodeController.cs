using System;
using System.Collections;
using UnityEngine;

public class CodeController : MonoBehaviour
{
    [SerializeField] private EnterCodeController codeController;
    [SerializeField] private GameObject correctCodeScreen;
    [SerializeField] private GameObject wrongCodeScreen;
    [SerializeField] private float feedbackTime = 0.8f;
    
    private string _correctCode;
    private Action _onComplete;
    
    public void Init(string correctCode, Action onComplete)
    {
        _correctCode = correctCode.ToUpper();
        codeController.Init(_correctCode, OnCodeAttempt);
        _onComplete = onComplete;
        HideScreens();
    }
    
    public void OnCodeAttempt(string code)
    {
        if (IsCorrect(code)) OnCodeCorrect();
        else OnCodeIncorrect(code);
    }

    private void OnCodeCorrect()
    {
        Debug.Log($"Code correct! ({_correctCode})");
        ShowCodeScreen(correctCodeScreen);
        StartCoroutine(DelayCallback(feedbackTime, _onComplete));
    }

    private void OnCodeIncorrect(string entered)
    {
        Debug.Log($"Code wrong! (Entered: {entered}. Expected: {_correctCode})");
        ShowCodeScreen(wrongCodeScreen);
        StartCoroutine(DelayCallback(feedbackTime, HideScreens));
    }

    private void ShowCodeScreen(GameObject codeScreen)
    {
        HideScreens();
        codeScreen.SetActive(true);
    }

    private void HideScreens()
    {
        correctCodeScreen.SetActive(false);
        wrongCodeScreen.SetActive(false);
    }

    private IEnumerator DelayCallback(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
    
    private bool IsCorrect(string code) => code == _correctCode;
}
