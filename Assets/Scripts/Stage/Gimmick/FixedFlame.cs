using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFlame : MonoBehaviour, IHitable
{
    [Tooltip("일반불 = 체크 해제 / 거센불 = 체크")]
    [SerializeField]
    private bool isStrongType = false;

    private void Awake()
    {
        SetBurningFlame(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Layers.Player)
        {
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                ExtraDamageInfo extraDamageInfo = new ExtraDamageInfo(transform.position);
                player.TakeDamage(1, extraDamageInfo);
            }
        }
    }

    private void SetBurningFlame(bool getState)
    {
        if(isStrongType)
        {
            if(getState)
            {
                Debug.Log("Create strong type fixed flame");
            }
            else
            {
                Debug.Log("Delete strong type fixed flame");
                Destroy(this);
            }
        }
        else
        {
            if(getState)
            {
                Debug.Log("Create normal type fixed flame");
            }
            else
            {
                Debug.Log("Delete normal type fixed flame");
                Destroy(this);
            }
        }
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
    {
        if(extraDamageInfo.elementType == ElementType.Water)
        {
            if (isStrongType && amount == 5)
            {
                SetBurningFlame(false);
            }
            else if (!isStrongType && amount == 1)
            {
                SetBurningFlame(false);
            }
        }
    }
}
