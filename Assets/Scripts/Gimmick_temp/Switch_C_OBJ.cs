using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 스위치랑 연결되는 오브젝트는 이 클래스를 상속받음 (문, 발판)
/// </summary>

public class Switch_C_OBJ : MonoBehaviour
{

    public List<Switchs_Ctrl> _switchs;
    public bool AllSwitchOn = false;

    public virtual void Activate() //문이 열리는 행동 혹은 발판의 움직임
    {

    }

    public void SwitchCheck() //연결된 스위치가 전부 켜져있는지 체크
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
