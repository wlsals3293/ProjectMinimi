using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SimpleManager<UIManager>
{
    // TODO �����Ƽ� �ϴ� �̷��� ���߿� �����տ��� ��������
    [SerializeField] private GameObject endPanel = null;
    [SerializeField] private GameObject deathPanel = null;

    protected override void Awake()
    {
        base.Awake();
    }

    // TODO ���뼺 �ְ� ����
    public void OpenUI_EndStage()
    {
        // TODO �Լ�ȭ
        Cursor.visible = true;                   
        Cursor.lockState = CursorLockMode.None;


        endPanel.SetActive(true);
        PlayerManager.Instance.PlayerCtrl.pause = true;
    }

    public void OpenUI_Death()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        deathPanel.SetActive(true);
    }

    // TODO �̰͵� �ӽ� UI Panel���� ��ũ��Ʈ ���� �����ϰ� �ֱ�
    public void Restart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        endPanel.SetActive(false);
        StageManager.Instance.RestartStage();
    }

    public void GameEnd()
    {
        Application.Quit();
    }

   
}
