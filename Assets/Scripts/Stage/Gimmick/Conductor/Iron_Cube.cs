using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron_Cube : ConductorBase, IHitable
{
    protected override void Awake()
    {
        base.Awake();

        overlapSize = new Vector3(2.5f, 2.5f, 2.5f);
        overlapLayer = LayerMasks.PO;
    }

    public void TakeDamage(int amount) { }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Electricity)
        {
            if (extraDamageInfo.damageCauser == null)
            {
                electricityEventInfo.source = this.transform;
                ElectricityManager.Instance.AddEventNum(out electricityEventInfo.EventNum);
            }

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
}
