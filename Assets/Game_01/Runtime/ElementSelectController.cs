using System;
using System.Linq;
using Game_01.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElementSelectController : MonoBehaviour
{
    [SerializeField] private StartGameLevelManager _levelManager;
    [SerializeField] private CycleActivateChildren _randomButtonCycler;

    private void Awake() => ElementButton.OnClicked += OnElementButtonClicked;
    private void OnDestroy() => ElementButton.OnClicked -= OnElementButtonClicked;

    private void OnElementButtonClicked(object sender, Element e) => PickElement(e);

    private void PickElement(Element element)
    {
        Debug.Log($"Element selected: {element}");
        _levelManager.EndGame(element);
    }
}
