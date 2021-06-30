using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : BaseManager<GameManager>
{


    private void Start()
    {
        StageManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
    }
}
