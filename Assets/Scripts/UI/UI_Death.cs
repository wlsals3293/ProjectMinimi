using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Death : UIView
{
    public void Respawn()
    {
        PlayerManager.Instance.RespawnPlayer();
        UIManager.Instance.CloseView();
    }
}
