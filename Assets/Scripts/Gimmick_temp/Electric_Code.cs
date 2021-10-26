using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Code : ConductorBase, IHitable
{
    public Transform Wire;

    public void TakeDamage(int amount) { }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        ElectricityManager.Instance.AddEventNum(out electricityEventInfo.EventNum);
        Wire.GetComponent<Electric_Wire>().ActivateElectricity(this.transform, electricityEventInfo.EventNum);
    }
}
