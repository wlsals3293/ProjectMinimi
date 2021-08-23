using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : BaseManager<GameManager>
{
    /// <summary>
    /// 스테이지 씬에서 바로 플레이할 때 체크. Editor Only
    /// </summary>
    public bool inGameStart = false;

    private bool escMenuOpened = false;



    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (inGameStart)
        {
            SceneManager.Instance.onLoadComplete += InGameInit;
            InGameInit();
        }

    }


    // 임시
    public void StartGame(ref string stageName)
    {
        SceneManager.Instance.onLoadComplete += InGameInit;
        SceneManager.Instance.LoadScene(ref stageName);
    }

    /// <summary>
    /// 인게임 초기화
    /// </summary>
    public void InGameInit()
    {
        StageManager.Instance.Initialize();
        UIManager.Instance.SceneInit();
        UIManager.Instance.InGameInit();
        PlayerManager.Instance.Initialize();
        MinimiManager.Instance.Initialize();
        CameraManager.Instance.Initialize();
    }

    public void ToggleESCMenu()
    {
        if (!escMenuOpened)
            UIManager.Instance.OpenView(UIManager.EUIView.EscMenu);
        else
            UIManager.Instance.CloseView();

        escMenuOpened = !escMenuOpened;
    }
}
