using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Activator, IHitable
{
    [Tooltip("체크되어 있으면 불이 붙은 상태가 기본 상태 " +
        "체크해제되어 있으면 불이 꺼진 상태가 기본 상태.")]
    [SerializeField]
    private bool defaultState = false;

    [Tooltip("일반 = 체크 해제 / 특수 = 체크")]
    [SerializeField]
    private bool isSpecial = false;


    [SerializeField]
    private Renderer switchRenderer;

    [SerializeField]
    private Material[] colorTemp;


    private TimerInstance timer;


    private void Start()
    {
        SetFire(defaultState);
    }

    public override bool Activate()
    {
        if (isSpecial)
        {
            if (timer == null)
                timer = Timer.SetTimer(this, OnSpecialType, 2.0f);
            else
                timer.Restart();
        }

        if (!base.Activate())
            return false;

        SetFire(!defaultState);
        return true;
    }

    public override bool Deactivate()
    {
        if (isSpecial && timer != null)
        {
            timer.Cancel();
            timer = null;
        }

        if (!base.Deactivate())
            return false;

        SetFire(defaultState);
        return true;
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            if (!defaultState) Activate();
            else Deactivate();
        }
        else if (extraDamageInfo.elementType == ElementType.Water)
        {
            if (defaultState) Activate();
            else Deactivate();
        }
    }

    private void OnSpecialType()
    {
        timer = null;
        Deactivate();
    }

    private void SetFire(bool value)
    {
        if(value)
        {
            // 불 켜짐
            if (switchRenderer != null && colorTemp[1] != null)
                switchRenderer.material = colorTemp[1];
        }
        else
        {
            // 불 꺼짐
            if (switchRenderer != null && colorTemp[0] != null)
                switchRenderer.material = colorTemp[0];
        }
    }
}