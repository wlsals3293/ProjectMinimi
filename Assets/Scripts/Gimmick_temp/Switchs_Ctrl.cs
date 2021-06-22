using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchs_Ctrl : MonoBehaviour
{
    public enum SwitchType { Maintain, OnOff }
    public SwitchType _type = SwitchType.OnOff;

    [HideInInspector]
    public Switch_C_OBJ _connetObj;

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
    Renderer _thisColor;

    

    // Start is called before the first frame update
    void Start()
    {
        _thisColor = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (
           other.gameObject.CompareTag("Player") ||
           other.gameObject.CompareTag("Minimi") ||
           other.gameObject.CompareTag("Object")
           )
        {
            isActivate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (
            other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Minimi") ||
            other.gameObject.CompareTag("Object")
            )
        {
            isActivate = false;
        }
    }

    public void Connecting(Switch_C_OBJ connectingObj)
    {
        _connetObj = connectingObj;
    }
}