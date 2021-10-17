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
    protected override void Awake()
    {
        base.Awake();
    }

    public void TestFlow(Transform giver, Transform taker)
    {
        IronObjectBase _giver = giver.GetComponent<IronObjectBase>();
        IronObjectBase _taker = taker.GetComponent<IronObjectBase>();

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
