using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game_01.Runtime;
using UnityEngine;

public class StopCycleOnClick : MonoBehaviour
{
    private ElementButton[] _buttons;
    private CycleActivateChildren _cycler;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        _cycler = GetComponent<CycleActivateChildren>();
        _buttons = GetComponentsInChildren<ElementButton>();
        ElementButton.OnClicked += OnElementButtonClicked;
    }

    private void OnDisable()
    {
        ElementButton.OnClicked -= OnElementButtonClicked;
    }

    private void OnElementButtonClicked(object sender, Element e)
    {
        if (_buttons.Contains(sender))
        {
            StopCycle();
        }
    }

    private void StopCycle()
    {
        if (!_cycler) return;
        _cycler.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
