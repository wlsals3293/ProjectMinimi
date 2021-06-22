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
    private bool isOpen;

    public float DoorSpd = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        thisDoor = GetComponent<Door_Ctrl>();

        _startPoint = transform.position;
        _endPotint = _startPoint - new Vector3(0, transform.lossyScale.y, 0);

        foreach(Switchs_Ctrl switchs in _switchs)
        {
            if(switchs._connetObj != null) { Debug.LogError("한개의 스위치는 한개의 오브젝트만 연결이 가능합니다."); }
            switchs.Connecting(thisDoor);
        }
    }


    public override void Activate()
    {
        if (AllSwitchOn) isOpen = true; //연결된 스위치가 전부 켜지면 isOpen = true

        else if (!AllSwitchOn && doorType == DoorType.OnOff) isOpen = false; //연결된 스위치 중 하나라도 꺼져있고 문의 타입이 OnOff형이면 isOpen = false

        if (isOpen) //isOpen이 true면 열림
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPotint, DoorSpd * Time.deltaTime); 
        }
        else //isOpen이 false면 닫힘
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
