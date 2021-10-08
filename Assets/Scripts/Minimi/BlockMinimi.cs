using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
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
        base.Install(targetPosition, targetRotation);

    }

    public override void Uninstall()
    {
        base.Uninstall();

    }

    public override void UpdateStatus()
    {
        // 자식 미니미 위치와 회전 조정
        for (int i = 0; i < childMinimis.Count; i++)
        {
            Vector3 pos = transform.position + transform.up * (STACK_HEIGHT * (i + 1));
            Quaternion rot = transform.rotation;
            childMinimis[i].Install(pos, rot);
        }
    }

    public override void SetBigState()
    {
        base.SetBigState();
        col.enabled = true;
    }

    public override void SetSmallState()
    {
        base.SetSmallState();
        col.enabled = false;
    }
}
