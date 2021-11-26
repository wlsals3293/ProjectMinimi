using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : Activator, IHitable
{
    [Tooltip("üũ�Ǿ� ������ Ȱ��ȭ �� �����ð��� ������ �ٽ� ��Ȱ��ȭ �˴ϴ�.")]
    [SerializeField]
    private bool autoRevert;

    [Tooltip("Ȱ��ȭ ���ӽð� (Auto Revert üũ��)")]
    [SerializeField]
    private float activationTime = 3.0f;

    [Tooltip("����ġ�� �۵��ϴ� ����. None���� �����ϸ� ��� ���ҿ� �۵�")]
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
