using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Activator, IHitable
{
    [Tooltip("üũ�Ǿ� ������ ���� ���� ���°� �⺻ ���� " +
        "üũ�����Ǿ� ������ ���� ���� ���°� �⺻ ����.")]
    [SerializeField]
    private bool defaultState = false;

    [Tooltip("�Ϲ� = üũ ���� / Ư�� = üũ")]
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
            // �� ����
            if (switchRenderer != null && colorTemp[1] != null)
                switchRenderer.material = colorTemp[1];
        }
        else
        {
            // �� ����
            if (switchRenderer != null && colorTemp[0] != null)
                switchRenderer.material = colorTemp[0];
        }
    }
}