using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : BaseManager<StageManager>
{

    /// <summary>
    /// �������� ����
    /// </summary>
    private StageInfo stageInfo = null;

    /// <summary>
    /// ���� üũ����Ʈ�� �ش��ϴ� ����Ʈ ��ȣ
    /// </summary>
    private int currentCheckpoint = 0;

    /// <summary>
    /// üũ����Ʈ ����Ʈ
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

    public void RestartStage()
    {
        currentCheckpoint = 0;

        PlayerManager.Instance.RespawnPlayer();
    }

    public void Initialize()
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

        currentCheckpoint = 0;

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

        Debug.Log($"üũ����Ʈ ������Ʈ. index:{index}");
    }

    public Transform GetLastCheckpoint()
    {
        if (currentCheckpoint < 0 || checkpoints == null ||
            currentCheckpoint >= checkpoints.Count)
            return null;

        return checkpoints[currentCheckpoint].transform;
    }
}
