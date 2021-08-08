using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 스위치랑 연결되는 오브젝트는 이 클래스를 상속받음 (문, 발판)
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

    public virtual void Activate() //문이 열리는 행동 혹은 발판의 움직임
    {

    }

    public virtual void WhenAllSwitchOn() //스위치가 다 켜졌을 때만 호출
    {

    }

    public void SwitchCheck() //연결된 스위치가 전부 켜져있는지 체크
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

    public void ConnectingSwitch() //스위치들과 연결
    {
        foreach (SwitchBase switchs in switchs) 
        {
            switchs.Connecting(GetComponent<Switch_C_OBJ>());
        }
    }
}
