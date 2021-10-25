using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : BaseManager<GameManager>
{
    /// <summary>
    /// �������� ������ �ٷ� �÷����� �� üũ. Editor Only
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

    private void Update()
    {
        Timer.UpdateTimers(Time.deltaTime);
    }

    // �ӽ�
    public void StartGame(ref string stageName)
    {
        SceneManager.Instance.onLoadComplete += InGameInit;
        SceneManager.Instance.LoadScene(ref stageName);
    }

    /// <summary>
    /// �ΰ��� �ʱ�ȭ
    /// </summary>
    public void InGameInit()
    {
        Timer.Initialize();
        StageManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
        UIManager.Instance.SceneInit();
        UIManager.Instance.InGameInit();
        CameraManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
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
