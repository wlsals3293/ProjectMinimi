using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageEnd : UIView
{


    public void Restart()
    {
        StageManager.Instance.RestartStage(true);
        UIManager.Instance.CloseView();
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
