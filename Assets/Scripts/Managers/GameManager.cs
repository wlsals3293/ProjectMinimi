using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : BaseManager<GameManager>
{
    [SerializeField] private bool inGameStart = false;

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


    public void InGameInit()
    {
        StageManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
        MinimiManager.Instance.Initialize();
        UIManager.Instance.InGameInit();
    }
}
