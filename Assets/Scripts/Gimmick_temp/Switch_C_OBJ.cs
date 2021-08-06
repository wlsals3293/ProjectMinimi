using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ġ�� ����Ǵ� ������Ʈ�� �� Ŭ������ ��ӹ��� (��, ����)
/// </summary>

public class Switch_C_OBJ : MonoBehaviour
{

    public List<Switchs_Ctrl> _switchs;
    public bool AllSwitchOn = false;

    public virtual void Activate() //���� ������ �ൿ Ȥ�� ������ ������
    {

    }

    public void SwitchCheck() //����� ����ġ�� ���� �����ִ��� üũ
    {
        int i = 0;
        foreach (Switchs_Ctrl switchs in _switchs)
        {
            if (switchs.isActivate)
            {
                i++;
            }
        }

        if (i >= _switchs.Count)
        {
            AllSwitchOn = true;
        }

        if (i < _switchs.Count)
        {
            AllSwitchOn = false;
        }
    }
}
