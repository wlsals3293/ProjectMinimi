using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : BaseManager<StageManager>
{

    /// <summary>
    /// 스테이지 정보
    /// </summary>
    private StageInfo stageInfo = null;

    /// <summary>
    /// 현재 체크포인트에 해당하는 리스트 번호
    /// </summary>
    private int currentCheckpoint = 0;

    /// <summary>
    /// 체크포인트 리스트
    /// </summary>
    private List<Checkpoint> checkpoints = null;



    public float GlobalKillY
    {
        get => stageInfo == null ? 5f : stageInfo.globalKillY;
    }



    protected override void Awake()
    {
        base.Awake();

    }


    /// <summary>
    /// 스테이지 재시작
    /// </summary>
    /// <param name="resetCheckpoint">true면 체크포인트 초기화</param>
    public void RestartStage(bool resetCheckpoint = false)
    {
        if (resetCheckpoint)
            currentCheckpoint = 0;

        SceneManager.Instance.ReloadScene();
    }

    public void Initialize()
    {
        // 스테이지 정보 체크
        if (stageInfo == null)
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

        for (int i = 0; i < checkpoints.Count; i++)
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

        Debug.Log($"체크포인트 업데이트. index:{index}");
    }

    public Transform GetLastCheckpoint()
    {
        if (currentCheckpoint < 0 || checkpoints == null ||
            currentCheckpoint >= checkpoints.Count)
            return null;

        return checkpoints[currentCheckpoint].transform;
    }
}
