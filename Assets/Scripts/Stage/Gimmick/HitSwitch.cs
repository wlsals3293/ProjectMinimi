using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : Activator, IHitable
{
    [Tooltip("체크되어 있으면 활성화 후 일정시간이 지나면 다시 비활성화 됩니다.")]
    [SerializeField]
    private bool autoRevert;

    [Tooltip("활성화 지속시간 (Auto Revert 체크시)")]
    [SerializeField]
    private float activationTime = 3.0f;

    [Tooltip("스위치가 작동하는 원소. None으로 설정하면 모든 원소에 작동")]
    [SerializeField]
    private ElementType triggerElement;

    [SerializeField]
    private Renderer switchRenderer;

    [SerializeField]
    private Material[] colorTemp;


    private TimerInstance timerInstance;


    public override bool Activate()
    {
        if (autoRevert && timerInstance != null)
            timerInstance.Restart();

        if (!base.Activate())
            return false;


        if (autoRevert && timerInstance == null)
            timerInstance = Timer.SetTimer(this, () => Deactivate(), activationTime);

        if (switchRenderer != null && colorTemp[1] != null)
            switchRenderer.material = colorTemp[1];

        return true;
    }

    public override bool Deactivate()
    {
        if (timerInstance != null)
        {
            timerInstance.Cancel();
            timerInstance = null;
        }

        if (!base.Deactivate())
            return false;

        if (switchRenderer != null && colorTemp[0] != null)
            switchRenderer.material = colorTemp[0];

        return true;
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
    {
        if (triggerElement == ElementType.None ||
            extraDamageInfo.elementType == triggerElement)
        {
            Activate();
        }
    }
}
