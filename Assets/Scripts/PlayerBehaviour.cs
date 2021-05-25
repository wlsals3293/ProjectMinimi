using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour
{
    private Transform trans = null;
    private const float RAY_DISTANCE = 5f;

    public PlayerBehaviour(Transform playerTrans)
    {
        trans = playerTrans;
    }

    public enum ActiveKeyType
    {
        Use // key E
    }


    public void UpdateActiveKeyAction(ActiveKeyType keyType)
    {
        switch (keyType)
        {
            case ActiveKeyType.Use:
                UpdateUseKey();
                break;
            //default:
            //    break;
        }
    }

    private void UpdateUseKey()
    {
        RaycastHit hit =  Raycast(RAY_DISTANCE);
        if (hit.collider == null)
            return;

        if(hit.collider.gameObject.layer == Layers.minimi)
        {
            Debug.Log("Did Hit Minimi");
            hit.collider.SendMessage(MinimiController.SEND_SETPIVOT, trans);
        }
    }

    public void DrawLineRaycatAllways(float distance = 0f)
    {
        if(distance != 0)
        {
            Raycast(distance);
        }
        else
        {
            Raycast(RAY_DISTANCE);
        }
    }

    private RaycastHit Raycast(float distance)
    {
        RaycastHit hit;
        Vector3 pos = trans.position + Vector3.up;

        if (Physics.Raycast(pos, trans.TransformDirection(Vector3.forward), out hit, RAY_DISTANCE))
        {
#if UNITY_EDITOR
            Debug.DrawLine(pos, pos + (trans.TransformDirection(Vector3.forward) * hit.distance), Color.red);
#endif
        }

        return hit;
    }

    private string GetHitTag()
    {
        return null;
    }
        
}
