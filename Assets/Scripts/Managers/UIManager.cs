using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SimpleManager<UIManager>
{
    // TODO 귀찮아서 일단 이렇게 나중에 프리팹에서 꺼내쓰기
    [SerializeField] private GameObject endPanel = null;
    [SerializeField] private GameObject deathPanel = null;

    protected override void Awake()
    {
        base.Awake();
    }

    // TODO 범용성 있게 수정
    public void OpenUI_EndStage()
    {
        // TODO 함수화
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

    // TODO 이것도 임시 UI Panel별로 스크립트 별도 생성하고 넣기
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
