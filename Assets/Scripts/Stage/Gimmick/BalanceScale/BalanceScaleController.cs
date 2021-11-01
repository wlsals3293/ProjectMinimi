using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScaleController : MonoBehaviour
{
    [SerializeField] private BalanceScale balanceScale;
    [SerializeField] private bool isLeft = true;

    private void OnTriggerStay(Collider other)
    {
        /*if (other.tag == Tags.Minimi) 
        {
            balanceScale.AddMinimi(other, isLeft);
        }*/
    }

}
