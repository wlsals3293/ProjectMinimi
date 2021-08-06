using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchs_Ctrl : MonoBehaviour
{
    [HideInInspector] public Switch_C_OBJ _connetObj;

    private bool is_activate = false;

    public bool isActivate
    {
        get { return is_activate; }
        set
        {
            is_activate = value;

            _connetObj.SwitchCheck();
            
            if (is_activate)
            {
                _thisColor.material = _color_DontTouch[0];
            }
            else
            {
                _thisColor.material = _color_DontTouch[1];
            }
        }
    }

    public Material[] _color_DontTouch;
    public Renderer _thisColor;
    


    public void Connecting(Switch_C_OBJ connectingObj)
    {
        _connetObj = connectingObj;
    }

}