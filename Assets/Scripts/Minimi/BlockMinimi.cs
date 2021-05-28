using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMinimi : Minimi
{
    private const float STACK_HEIGHT = 1.0f;

    private BoxCollider col;


    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    public override void Install(Vector3 targetPosition, Quaternion targetRotation)
    {
        BlockMinimi foundMinimi = GetMergeableMinimi(targetPosition, MERGE_DISTANCE, MinimiType.Block) as BlockMinimi;
        if(foundMinimi != null)
        {
            Merge(foundMinimi);
        }
        else
        {
            transform.SetPositionAndRotation(targetPosition, targetRotation);
        }
    }

    public override void UpdateStatus()
    {
        for (int i = 0; i < childMinimis.Count; i++)
        {
            Vector3 pos = transform.position + transform.up * (STACK_HEIGHT * i + 1);
            Quaternion rot = transform.rotation;
            childMinimis[i].transform.SetPositionAndRotation(pos, rot);
        }
    }

}
