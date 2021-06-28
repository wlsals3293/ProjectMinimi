using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SimpleManager<StageManager>
{
    // TODO 임시변수
    [SerializeField] private Transform startPos = null;

    /// <summary>
    /// 스테이지 정보
    /// </summary>
    private StageInfo stageInfo = null;

    /// <summary>
    /// 현재 체크포인트에 해당하는 리스트 번호
    /// </summary>
    private int currentCheckpoint = -1;

    /// <summary>
    /// 체크포인트 리스트
    /// </summary>
    private List<Checkpoint> checkpoints = null;
    

    public Vector3 StartPosition
    {
        get => startPos.position;
    }

    public float globalKillY
    {
        get => stageInfo == null ? 5f : stageInfo.globalKillY;
    }


    protected override void Awake()
    {
        base.Awake();

    }

    public void RestartStage()
    {
        PlayerManager.Instance.InitStagePlayer();

        currentCheckpoint = -1;
    }

    public void StageInitialize()
    {
        if(stageInfo == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(Tags.stageInfo);
            if (obj == null)
                return;

            StageInfo info = obj.GetComponent<StageInfo>();
            if (info == null)
                return;

            stageInfo = info;
        }

        InitializeCheckpoints(stageInfo.checkpoints);
    }

    private void InitializeCheckpoints(List<Checkpoint> inCheckpoints)
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

        MinimiManager.Instance.UnInstallAllMinimis();

        Debug.Log("체크포인트 업데이트");
    }

    public Transform GetCurrentCheckpoint()
    {
        if (checkpoints == null || currentCheckpoint < 0 ||
            currentCheckpoint >= checkpoints.Count)
            return null;

        return checkpoints[currentCheckpoint].transform;
    }
}
