using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public int index = -1;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.player))
        {
            StageManager.Instance.UpdateCheckpoint(index);
        }
    }


}
