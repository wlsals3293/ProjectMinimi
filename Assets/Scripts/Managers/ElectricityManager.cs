using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ElectricityEventInfo
{
    public Transform source;
    public int EventNum;
}

public class ElectricityManager : BaseManager<ElectricityManager>
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

    public void TestFlow(Transform giver, Transform taker)
    {
        ConductorBase _giver = giver.GetComponent<ConductorBase>();
        ConductorBase _taker = taker.GetComponent<ConductorBase>();

        if (_giver.electricityEventInfo.EventNum == _taker.electricityEventInfo.EventNum)
        {
            return;
        }
        else
        {
            if (_giver.electricityEventInfo.EventNum > _taker.electricityEventInfo.EventNum)
            {
                _taker.electricityEventInfo.EventNum = _giver.electricityEventInfo.EventNum;

                ExtraDamageInfo extraDamageInfo = new ExtraDamageInfo(Vector3.zero, _giver.curElementType, _giver.transform);
                _taker.GetComponent<IHitable>()?.TakeDamage(0, extraDamageInfo);
            }
            else
                return;
        }
    }
}
