using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitch : SwitchBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsActivate = true;
        }
    }
}
