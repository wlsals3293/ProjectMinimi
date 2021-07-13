using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.player)
        {
            Debug.LogError("End");
            StageEndProcess();
        }
    }


    private void StageEndProcess()
    {
        PlayerManager.Instance.PlayerCtrl.pause = true;
        UIManager.Instance.OpenView(UIManager.EUIView.StageEnd);
    }

   
}
