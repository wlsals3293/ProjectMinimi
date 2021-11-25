using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricCircuit : ConductorBase
{
    [SerializeField]
    private ElectricWire wire;

    [SerializeField]
    private List<ElectricCode> codes = new List<ElectricCode>();

#if UNITY_EDITOR
    private List<ElectricCode> codes_copy = new List<ElectricCode>();
#endif

    [SerializeField]
    private ElectricSwitch[] switches;


    public List<ElectricCode> Codes { get => codes; }


#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (ElectricCode code in codes)
        {
            if (code == null)
                continue;

            if (code.circuit != this)
            {
                code.circuit = this;
            }
        }
    }
#endif

    protected override void Activate()
    {
        elapsedElectricityTime = 0f;
        if (onActivateCor == null)
        {
            onActivateCor = StartCoroutine(OnActivateElectricity());
        }

        // �ڵ�鿡 ����
        for (int i = 0; i < codes.Count; i++)
        {
            codes[i].Conduct(this);
        }

        if (isActive)
            return;

        // ���̾� Ȱ��ȭ
        wire.Activate();

        // ����ġ�� Ȱ��ȭ
        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].Activate();
        }

        isActive = true;
    }

    protected override void Deactivate()
    {
        if (onActivateCor != null)
        {
            StopCoroutine(onActivateCor);
            onActivateCor = null;
        }

        if (!isActive)
            return;

        // ���̾� ��Ȱ��ȭ
        wire.Deactivate();

        // ����ġ�� ��Ȱ��ȭ
        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].Deactivate();
        }

        isActive = false;
    }

    protected override IEnumerator OnActivateElectricity()
    {
        while (elapsedElectricityTime < HOLDING_TIME)
        {
            elapsedElectricityTime += Time.deltaTime;

            yield return null;
        }

        onActivateCor = null;
        Deactivate();
    }
}
