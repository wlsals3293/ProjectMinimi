using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Death : MonoBehaviour
{
    public void Respawn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        gameObject.SetActive(false);
        PlayerManager.Instance.RespawnPlayer();
    }
}
