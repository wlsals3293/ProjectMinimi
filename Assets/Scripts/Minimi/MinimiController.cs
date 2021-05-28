using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimiController : MonoBehaviour
{
    public const string SEND_SETPIVOT = "SetPivotPosition";

    [SerializeField] private Transform pivot = null;


    public void SetPivotPosition(Transform trans)
    {
        if(pivot != null && trans != null)
        {
            trans.position = pivot.position;
        }
    }
}
