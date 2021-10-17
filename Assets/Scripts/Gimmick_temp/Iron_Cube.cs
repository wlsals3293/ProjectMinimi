using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron_Cube : IronObjectBase, IHitable, IInteractable
{
    protected override void Awake()
    {
        base.Awake();

        overlapLayer = LayerMasks.PO;
    }

    public void TakeDamage(int amount)
    {

    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Electricity)
        {
            if (extraDamageInfo.damageCauser == null)
            {
                electricityEventInfo.source = this.transform;
                electricityEventInfo.EventNum++;
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

    public void Interact(PlayerController player)
    {
        player.Drag(this.transform);
    }
}
