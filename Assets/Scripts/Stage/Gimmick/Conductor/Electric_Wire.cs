using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Wire : ConductorBase
{
    [Header("전기 전달할 스위치")]
    public Transform Switch;
    [HideInInspector] public Transform Code = null;

    protected override void Awake()
    {
        base.Awake();
        
        overlapSize = new Vector3(2, 1.2f, 6);
        overlapLayer = LayerMasks.PO;
    }

    public void ActivateElectricWire(Transform _code, int _eventNum)
    {
        if (Code == null && _code != null) Code = _code;
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

    protected override void DetectOtherColliders()
    {
        Collider[] _overlapedCols = Physics.OverlapBox
        (
            transform.position,
            overlapSize / 2,
            transform.rotation,
            LayerMasks.Object
        );

        for (int i = 0; i < _overlapedCols.Length; i++)
        {
            if (_overlapedCols[i].transform == this.transform)
                continue;

            if (_overlapedCols[i].CompareTag("Conductor"))
            {
                ConductorBase otherConductor = _overlapedCols[i].GetComponent<ConductorBase>();
                if (otherConductor != null)
                    ElectricityManager.Instance.ElectricityProcess(this, otherConductor);
            }
            else if(_overlapedCols[i].GetComponent<Electric_Wire>())
            {
                ElectricityManager.Instance.ElectricityProcess_Wire(this.transform, _overlapedCols[i].transform);
            }
            else if(_overlapedCols[i].transform == Switch)
            {
                ElectricityManager.Instance.ElectricityProcess_Switch(this.transform, Switch);
            }
        }
    }
}
