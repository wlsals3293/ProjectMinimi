using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SimpleManager<StageManager>
{
    // TODO �ӽú���
    [SerializeField] private Transform startPos = null;


    public Vector3 StartPosition
    {
        get => startPos.position;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void RestartStage()
    {
        PlayerManager.Instance.InitStagePlayer();
    }
}
