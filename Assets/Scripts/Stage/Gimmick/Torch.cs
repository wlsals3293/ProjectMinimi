using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IHitable
{
    [Tooltip("활성화 여부")]
    [SerializeField]
    private bool activate = false;

    [Tooltip("일반 = 체크 해제 / 특수 = 체크")]
    [SerializeField]
    private bool isSpecial = false;
    [HideInInspector]
    public bool burn = false;

    private TimerInstance timer;

    private void Awake()
    {
        if(activate)
        {
            SetBurningTorch(true);
        }
    }

    private void SetBurningTorch(bool getActive)
    {
        if(getActive)
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

    private void OnSpecialType()
    {
        SetBurningTorch(activate);
        timer = null;
    }

    public void TakeDamage(int amount)
    {
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if(!burn && extraDamageInfo.elementType == ElementType.Fire)
        {
            if (timer != null)
            {
                timer.Renew();
            }

            SetBurningTorch(true);
        }
        else if (burn && extraDamageInfo.elementType == ElementType.Water)
        {
            if (timer != null)
            {
                timer.Renew();
            }

            SetBurningTorch(false);
        }

        if (isSpecial)
        {
            timer = Timer.SetTimer(this, OnSpecialType, 2.0f);
        }
    }
}