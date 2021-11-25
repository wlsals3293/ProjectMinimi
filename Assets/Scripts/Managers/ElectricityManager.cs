using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ElectricityManager : ManagerBase<ElectricityManager>
{
    //Queue<ElectricityEventInfo> electricityEventQueue;
    int latestEventNum = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddEventNum(out int _eventNum)
    {
        latestEventNum++;
        _eventNum = latestEventNum;
    }

    /*public void ElectricityProcess(ConductorBase giver, ConductorBase taker)
    {
        if (!giver || !taker) return;

        if (giver.electricityEventInfo.EventNum > taker.electricityEventInfo.EventNum)
        {
            taker.electricityEventInfo.EventNum = giver.electricityEventInfo.EventNum;

            ExtraDamageInfo extraDamageInfo = new ExtraDamageInfo(Vector3.zero, ElementType.Electricity, giver.transform);
            taker.GetComponent<IHitable>()?.TakeDamage(0, extraDamageInfo);
        }
    }

    public void ElectricityProcess_Wire(Transform giver, Transform taker)
    {
        Electric_Wire _giver = giver.GetComponent<Electric_Wire>();
        Electric_Wire _taker = taker.GetComponent<Electric_Wire>();

        if (!_giver || !_taker) return;

        if (_giver.electricityEventInfo.EventNum == _taker.electricityEventInfo.EventNum)
        {
            return;
        }
        else if (_giver.electricityEventInfo.EventNum > _taker.electricityEventInfo.EventNum)
        {
            _taker.ActivateElectricWire(null, _giver.electricityEventInfo.EventNum);
        }
    }

    public void ElectricityProcess_Switch(Transform giver, Transform taker)
    {
        Electric_Wire _giver = giver.GetComponent<Electric_Wire>();
        Electric_Switch _taker = taker.GetComponent<Electric_Switch>();

        if (!_giver || !_taker) return;

        if (_giver.electricityEventInfo.EventNum == _taker.electricityEventInfo.EventNum)
        {
            return;
        }
        else if (_giver.electricityEventInfo.EventNum > _taker.electricityEventInfo.EventNum)
        {
            _taker.electricityEventInfo.EventNum = _giver.electricityEventInfo.EventNum;

            _taker.ActivateElectricSwitch();
        }
    }*/
}
