using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Title : MonoBehaviour
{
    private string stageName = "temp";
    
    public void GameStart()
    {
        GameManager.Instance.StartGame(ref stageName);
    }
}
