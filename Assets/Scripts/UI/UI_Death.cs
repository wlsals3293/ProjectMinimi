using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Death : UIView
{

    public void Respawn()
    {
        StageManager.Instance.RestartStage();
        UIManager.Instance.CloseView();
    }
}
