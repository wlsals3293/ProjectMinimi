using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBase : MonoBehaviour
{
    [HideInInspector]public List<Switch_C_OBJ> connectObjs;

    private bool isActivate = false;

    private RadioSwitchBundle radio;

    private SwitchBase thisSwitch;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;

            foreach (Switch_C_OBJ obj in connectObjs)
            {
                obj.SwitchCheck();
            }
            
            if (isActivate)
            {
                _thisColor.material = _color_DontTouch[0];
                if(radio != null)
                {
                    radio.TurnOffOtherSwitchs(thisSwitch);
                }
            }
            else
            {
                _thisColor.material = _color_DontTouch[1];
            }
        }
    }

    public Material[] _color_DontTouch;
    public Renderer _thisColor;

    private void Awake()
    {
        thisSwitch = GetComponent<SwitchBase>();
    }

    public void ConnectToOBJ(Switch_C_OBJ connectingObj)
    {
        connectObjs.Add(connectingObj);
    }

    public void ConnectToBundle(RadioSwitchBundle connect)
    {
        radio = connect;
    }
}