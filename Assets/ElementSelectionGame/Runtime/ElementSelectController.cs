using System;
using System.Linq;
using Game_01.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ElementSelectController : MonoBehaviour
{
    [FormerlySerializedAs("_levelManager")] [SerializeField] private ElementSelectGameController miniGameController;
    [SerializeField] private CycleActivateChildren _randomButtonCycler;

    private void Awake() => ElementButton.OnClicked += OnElementButtonClicked;
    private void OnDestroy() => ElementButton.OnClicked -= OnElementButtonClicked;

    private void OnElementButtonClicked(object sender, Element e) => PickElement(e);

    private void PickElement(Element element)
    {
        Debug.Log($"Element selected: {element}");
        miniGameController.EndGame(element);
    }
}
