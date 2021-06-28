using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : SimpleManager<GameManager>
{


    private void Start()
    {
        StageManager.Instance.StageInitialize();
        PlayerManager.Instance.InitStagePlayer();
    }
}
