using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : Activator, IHitable
{
    [Tooltip("활성화 시간")]
    [SerializeField]
    private float activationTime = 3.0f;

    [SerializeField]
    private Renderer switchRenderer;

    [SerializeField]
    private Material[] colorTemp;


    private TimerInstance timerInstance;


    public override void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            for (int i = 0; i < activatees.Count; i++)
            {
                if (activatees[i] != null)
                    activatees[i].ReceiveSignal(true);
            }
            if (radioGroup != null)
            {
                radioGroup.ActivateOnly(this);
            }

            if (timerInstance == null)
                timerInstance = Timer.SetTimer(this, () => Deactivate(), activationTime);

            if (switchRenderer != null && colorTemp[1] != null)
                switchRenderer.material = colorTemp[1];
        }
        else
        {
            if (timerInstance != null)
                timerInstance.Restart();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if (timerInstance != null)
        {
            timerInstance.Cancel();
            timerInstance = null;
        }

        if (switchRenderer != null && colorTemp[0] != null)
            switchRenderer.material = colorTemp[0];
    }

    public void TakeDamage(int amount)
    {
        Activate();
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        Activate();
    }
}
