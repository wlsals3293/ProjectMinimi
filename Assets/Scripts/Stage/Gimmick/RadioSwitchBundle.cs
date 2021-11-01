using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadioSwitchBundle : MonoBehaviour
{
    public List<SwitchBase> baseSwitchs;


    private List<SwitchBase> switchs;

    private RadioSwitchBundle thisBundle;

    private void Start()
    {
        thisBundle = GetComponent<RadioSwitchBundle>();

        switchs = baseSwitchs.Distinct().ToList(); //인스펙터를 통해서 추가한 스위치리스트에서 중복제거

        foreach(SwitchBase obj in switchs)
        {
            obj.ConnectToBundle(thisBundle);
        }
    }

    public void TurnOffOtherSwitchs(SwitchBase activateSwitch)
    {
        foreach(SwitchBase obj in switchs)
        {
            if(obj != activateSwitch)
            {
                obj.IsActivate = false;
            }
        }
    }

}
