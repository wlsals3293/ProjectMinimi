using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitch : Activator
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            Activate();
        }
    }
}
