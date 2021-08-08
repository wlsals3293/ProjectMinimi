using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBase : MonoBehaviour
{
    [HideInInspector] public Switch_C_OBJ connetObj;

    private bool isActivate = false;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;

            connetObj.SwitchCheck();
            
            if (isActivate)
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
        connetObj = connectingObj;
    }

}