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
    private bool alreadyOff = true;
    public bool AllSwitchOn
    {
        get { return allSwitchOn; }
        set
        {
            allSwitchOn = value;
            if (allSwitchOn)
            {
                Activate();
                alreadyOff = false;
            }
            if (!allSwitchOn)
            {
                if (!alreadyOff)
                {
                    Deactivate();
                    alreadyOff = true;
                }
            }
        }

    }
    public virtual void Activate() //스위치가 다 켜지면 한번 호출
    {

    }

    

    public virtual void Deactivate() //스위치가 다 켜진 상태에서 스위치가 하나라도 꺼지면 한번 호출
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
            switchs.ConnectToOBJ(GetComponent<Switch_C_OBJ>());
        }
    }
}
