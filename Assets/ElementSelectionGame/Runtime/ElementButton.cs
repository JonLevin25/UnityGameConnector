using System;
using UnityEngine;

public class ElementButton : MonoBehaviour
{
    [SerializeField] private Element _element;

    public static event EventHandler<Element> OnClicked;

    // Called from scene
    public void OnButtonClicked()
    {
        Debug.Log($"Element Button clicked ({_element})", gameObject);
        OnClicked?.Invoke(this, _element);
    }
}
