using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Switch : ConductorBase
{
    [Header("스위치 타입")]
    public ElectricSwitchDurationType durationType;
    public Renderer switchRenderer;

    protected override void Awake()
    {
        rendererComponent = switchRenderer;
        curElementType = ElementType.None;
    }

    public void ActivateElectricSwitch()
    {
        switch(durationType)
        {
            case ElectricSwitchDurationType.Continuation:
                IsActivate = true;
                break;
            case ElectricSwitchDurationType.OnOff:
                if (!IsActivate)
                {
                    StartCoroutine(OnActivateElectricity());
                }
                else
                {
                    curElectricityTime = Time.time;
                }
                break;
            default:
                break;
        }
    }

    protected override IEnumerator OnActivateElectricity()
    {
        IsActivate = true;

        while (true)
        {
            if (DebugFlag)
                Debug.LogWarning("Name: " + this.gameObject.name + "  Electricity Remain: " + (Time.time - curElectricityTime));

            if (Time.time - curElectricityTime >= electricity_Interval)
            {
                IsActivate = false;
                break;
            }

            // Do Something

            yield return null;
        }
    }
}
