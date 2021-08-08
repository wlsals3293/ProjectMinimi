using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ġ�� ����Ǵ� ������Ʈ�� �� Ŭ������ ��ӹ��� (��, ����)
/// </summary>

public class Switch_C_OBJ : MonoBehaviour
{

    public List<SwitchBase> switchs;
    private bool allSwitchOn = false;

    public bool AllSwitchOn
    {
        get { return allSwitchOn; }
        set
        {
            allSwitchOn = value;
            if (allSwitchOn)
            {
                WhenAllSwitchOn();
            }
        }
    }

    public virtual void Activate() //���� ������ �ൿ Ȥ�� ������ ������
    {

    }

    public virtual void WhenAllSwitchOn() //����ġ�� �� ������ ���� ȣ��
    {

    }

    public void SwitchCheck() //����� ����ġ�� ���� �����ִ��� üũ
    {
        int i = 0;
        foreach (SwitchBase switchs in switchs)
        {
            if (switchs.IsActivate)
            {
                i++;
            }
        }

        if (i >= switchs.Count)
        {
            AllSwitchOn = true;
        }

        if (i < switchs.Count)
        {
            AllSwitchOn = false;
        }
    }

    public void ConnectingSwitch() //����ġ��� ����
    {
        foreach (SwitchBase switchs in switchs) 
        {
            switchs.Connecting(GetComponent<Switch_C_OBJ>());
        }
    }
}
