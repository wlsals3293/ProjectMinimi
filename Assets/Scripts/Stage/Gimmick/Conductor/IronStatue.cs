using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronStatue : ConductorBase, IHitable, IInteractable
{
    public void Interact(PlayerController player)
    {
        player.Hold(transform);
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
    {
        if (extraDamageInfo.elementType == ElementType.Electricity)
        {
            if (extraDamageInfo.damageCauser == null)
            {
                electricityEventInfo.source = transform;
                electricityEventInfo.eventNum = ++latestEventNum;
            }

            Activate();
        }
    }
}
