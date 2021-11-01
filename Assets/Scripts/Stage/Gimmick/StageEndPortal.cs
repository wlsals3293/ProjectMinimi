using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            StageManager.Instance.EndStage();
        }
    }
}
