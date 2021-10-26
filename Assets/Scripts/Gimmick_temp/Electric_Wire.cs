using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Wire : ConductorBase
{
    public Transform Switch;

    [HideInInspector] public Transform Code = null;

    protected override void Awake()
    {
        base.Awake();

        overlapSize = new Vector3(2, 1.2f, 6);
        overlapLayer = LayerMasks.PO;
    }

    public void ActivateElectricity(Transform _code, int _eventNum)
    {
        if (Code == null) Code = _code;
        electricityEventInfo.EventNum = _eventNum;

        if (!IsActivate)
        {
            StartCoroutine(OnActivateElectricity());
        }
        else
        {
            curElectricityTime = Time.time;
        }
    }
}
