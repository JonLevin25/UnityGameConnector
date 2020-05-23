using System;
using UnityEngine;

public class SafeButton : MonoBehaviour
{
    [SerializeField] private CodeController codeController;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("Safe clicked!");
        codeController.gameObject.SetActive(true);
    }
}
