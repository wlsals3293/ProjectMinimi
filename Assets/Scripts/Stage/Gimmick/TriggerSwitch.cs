using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectMinimi;

public class TriggerSwitch : Activator
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            Deactivate();
        }
    }
}
