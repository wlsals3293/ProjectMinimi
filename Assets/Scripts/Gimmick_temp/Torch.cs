using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IHitable
{
    [Tooltip("활성화 여부")]
    [SerializeField]
    private bool active = false;
    private bool burn = false;

    private void TorchStatus(bool set_Active)
    {
        if(set_Active)
        {
            burn = true;
            Debug.Log("Create fire effect");
        }
        else
        {
            burn = false;
            Debug.Log("Delect fire effect");
        }
    }

    private void Awake()
    {
        TorchStatus(active);
    }

    public void TakeDamage(int amount)
    {
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if(extraDamageInfo.elementType == ElementType.Fire)
        {
            TorchStatus(true);
        }
        else if (burn && extraDamageInfo.elementType == ElementType.Water)
        {
            TorchStatus(false);
        }
    }
}