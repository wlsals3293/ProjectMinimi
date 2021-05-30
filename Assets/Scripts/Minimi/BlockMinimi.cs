using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMinimi : Minimi
{
    private const float STACK_HEIGHT = 2.0f;

    private BoxCollider col;


    private void Awake()
    {
        minimiType = MinimiType.Block;
        col = GetComponent<BoxCollider>();
    }

    public override void Install(Vector3 targetPosition, Quaternion targetRotation)
    {
        BlockMinimi foundMinimi = GetMergeableMinimi(targetPosition, MinimiManager.MERGE_DISTANCE, MinimiType.Block) as BlockMinimi;
        if(foundMinimi != null)
        {
            Merge(foundMinimi);
        }
        else
        {
            transform.SetPositionAndRotation(targetPosition, targetRotation);
        }

        col.enabled = true;
        isInstalled = true;
    }

    public override void Uninstall()
    {
        col.enabled = false;
        isInstalled = false;
    }

    public override void UpdateStatus()
    {
        // 자식 미니미 위치와 회전 조정
        for (int i = 0; i < childMinimis.Count; i++)
        {
            Vector3 pos = transform.position + transform.up * (STACK_HEIGHT * i + 1);
            Quaternion rot = transform.rotation;
            childMinimis[i].transform.SetPositionAndRotation(pos, rot);
        }
    }

}
