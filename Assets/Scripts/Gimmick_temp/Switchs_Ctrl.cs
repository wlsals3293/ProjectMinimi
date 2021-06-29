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

    private Coroutine onActivateCort;
    private BoxCollider boxTrigger;

    private LayerMask layerMask;


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



    private void Awake()
    {
        boxTrigger = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _thisColor = GetComponent<Renderer>();

        layerMask = LayerMask.GetMask("Minimi", "Player", "Object");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (
           other.gameObject.CompareTag("Player") ||
           other.gameObject.CompareTag("Minimi") ||
           other.gameObject.CompareTag("Object")
           )
        {
            isActivate = true;

            onActivateCort = StartCoroutine(OnActivate());
        }
    }

    public void Connecting(Switch_C_OBJ connectingObj)
    {
        _connetObj = connectingObj;
    }

    // 임시. 나중에 개편 예정
    private IEnumerator OnActivate()
    {
        while(true)
        {
            Collider[] cols = Physics.OverlapBox(transform.position + boxTrigger.center, boxTrigger.size, transform.rotation, layerMask, QueryTriggerInteraction.Ignore);

            bool isEmpty = true;

            foreach(Collider col in cols)
            {
                if(col.CompareTag("Player") || col.CompareTag("Minimi") || col.CompareTag("Object"))
                {
                    isEmpty = false;
                    break;
                }
            }

            if(isEmpty)
            {
                isActivate = false;
                StopCoroutine(onActivateCort);
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}