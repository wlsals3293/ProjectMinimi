using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : ManagerBase<StageManager>
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


    /// <summary>
    /// �������� �����
    /// </summary>
    /// <param name="resetCheckpoint">true�� üũ����Ʈ �ʱ�ȭ</param>
    public void RestartStage(bool resetCheckpoint = false)
    {
        if (resetCheckpoint)
            currentCheckpoint = 0;

        SceneManager.Instance.ReloadScene();
    }

    /// <summary>
    /// �������� ������
    /// </summary>
    public void EndStage()
    {
        Debug.Log("Stage End");
        PlayerManager.Instance.PlayerCtrl.pause = true;
        UIManager.Instance.OpenView(UIManager.EUIView.StageEnd);
    }

    public void Initialize()
    {
        // �������� ���� üũ
        if (stageInfo == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(Tags.StageInfo);
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

        Debug.Log($"üũ����Ʈ ������Ʈ. index:{index}");
    }

    public Transform GetLastCheckpoint()
    {
        if (currentCheckpoint < 0 || checkpoints == null ||
            currentCheckpoint >= checkpoints.Count)
            return null;

        return checkpoints[currentCheckpoint].transform;
    }

    public Transform GetCheckpoint(int index)
    {
        if(checkpoints == null || index < 0 || index >= checkpoints.Count)
        {
            return null;
        }

        return checkpoints[index].transform;
    }
}
