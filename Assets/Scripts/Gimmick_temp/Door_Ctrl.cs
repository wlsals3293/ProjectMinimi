using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door_Ctrl : Switch_C_OBJ
{
    public enum DoorType {Maintain, OnOff}
    public DoorType doorType;
    private Door_Ctrl thisDoor;
    private Vector3 _startPoint;
    private Vector3 _endPotint;
    
    bool isOpen = false;

    public float DoorSpd = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        thisDoor = GetComponent<Door_Ctrl>();

        _startPoint = transform.position;
        _endPotint = _startPoint - new Vector3(0, transform.lossyScale.y, 0);

        foreach(Switchs_Ctrl switchs in _switchs)
        {
            if(switchs._connetObj != null) { Debug.Log("한개의 스위치는 한개의 오브젝트까지만 연결이 가능합니다."); }
            switchs.Connecting(thisDoor);
        }
    }

    public override void SwitchCheck()
    {
        int i = 0;
        foreach(Switchs_Ctrl switchs in _switchs)
        {
            if (switchs.isActivate)
            {
                i++;
            }
        }

        if(i >= _switchs.Count)
        {
            isOpen = true;
        }

        if(i < _switchs.Count && doorType == DoorType.OnOff)
        {
            isOpen = false;
        }
        
        
       
    }

    public override void Activate()
    {
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPotint, DoorSpd * Time.deltaTime);
        }

        if(!isOpen && doorType == DoorType.OnOff)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPoint, DoorSpd * Time.deltaTime);
        }

    }
    // Update is called once per frame
    void Update()
    {
        Activate();
    }
}
