using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIPowerProgress : MonoBehaviour
{
    [SerializeField] private Data data;
    [SerializeField] private VisualTreeAsset healthCanvas;
    private ProgressBar power;
    private UIDocument uiDoc;
    void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        power = uiDoc.rootVisualElement.Q<ProgressBar>("PowerProgress");
        power.lowValue = 0;
        power.highValue = data.forceMinMax.y;
        data.OnPowerChanged += OnPowerChanged;
        Application.quitting += Unsubscribe;
        power.title = "";
    }

    private void Unsubscribe()
    {
        data.OnPowerChanged -= OnPowerChanged;
    }

    private void OnPowerChanged(float value)
    {
        power.lowValue = value;
    }


}
