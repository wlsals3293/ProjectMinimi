using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SimpleManager<StageManager>
{
    // TODO 임시변수
    [SerializeField] private Transform startPos = null;

    private int currentCheckpoint = -1;
    private List<Checkpoint> checkpoints = null;


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

        if(checkpoints != null)
            currentCheckpoint = checkpoints.Count - 1;
    }

    public void InitializeCheckpoints(List<Checkpoint> inCheckpoints)
    {
        checkpoints = inCheckpoints;

        if (inCheckpoints == null || inCheckpoints.Count <= 0)
        {
            currentCheckpoint = -1;
            return;
        }

        if (currentCheckpoint >= checkpoints.Count)
        {
            currentCheckpoint = checkpoints.Count - 1;
        }

        for (int i=0; i<checkpoints.Count; i++)
        {
            checkpoints[i].index = i;
        }
    }

    public void UpdateCheckpoint(int index)
    {
        if (index <= currentCheckpoint)
            return;

        currentCheckpoint = index;
    }

    public Transform GetCurrentCheckpoint()
    {
        if (checkpoints == null || currentCheckpoint < 0 ||
            currentCheckpoint >= checkpoints.Count)
            return null;

        return checkpoints[currentCheckpoint].transform;
    }
}
