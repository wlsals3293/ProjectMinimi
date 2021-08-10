using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ESC : UIView
{
    public void Resume()
    {
        GameManager.Instance.ToggleESCMenu();
    }

    public void Restart()
    {
        StageManager.Instance.RestartStage(true);
        GameManager.Instance.ToggleESCMenu();
    }

    public void RestartCheckpoint()
    {
        StageManager.Instance.RestartStage();
        GameManager.Instance.ToggleESCMenu();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
