using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronCube : ConductorBase, IHitable, IInteractable
{
    public void Interact(PlayerController player)
    {
        player.Drag(transform);
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
