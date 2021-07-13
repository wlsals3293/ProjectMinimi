using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : BaseManager<GameManager>
{
    public bool inGameStart = false;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (inGameStart)
            InGameInit();
    }


    // юс╫ц
    public void StartGame(ref string stageName)
    {
        SceneManager.Instance.onLoadComplete += InGameInit;
        SceneManager.Instance.LoadStage(ref stageName);
    }

    public void InGameInit()
    {
        StageManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
        MinimiManager.Instance.Initialize();
        UIManager.Instance.InGameInit();
        CameraManager.Instance.CurrentCameraCtrl.Initialize();
    }


}
