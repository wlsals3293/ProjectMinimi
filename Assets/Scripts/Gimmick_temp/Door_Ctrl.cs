using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door_Ctrl : Switch_C_OBJ
{
    public enum DoorType {Maintain, OnOff}
    public DoorType doorType;
    private Vector3 startPoint;
    private Vector3 endPotint;
    private bool isOpen;

    public float doorSpd = 4.0f;
    void Start()
    { 
        startPoint = transform.position;
        endPotint = startPoint - new Vector3(0, transform.lossyScale.y, 0);

        ConnectingSwitch();
    }


    public override void Activate()
    {
        if (AllSwitchOn) isOpen = true; //연결된 스위치가 전부 켜지면 isOpen = true

        else if (!AllSwitchOn && doorType == DoorType.OnOff) isOpen = false; //연결된 스위치 중 하나라도 꺼져있고 문의 타입이 OnOff형이면 isOpen = false

        if (isOpen) //isOpen이 true면 열림
        {
            transform.position = Vector3.MoveTowards(transform.position, endPotint, doorSpd * Time.deltaTime); 
        }
        else //isOpen이 false면 닫힘
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint, doorSpd * Time.deltaTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Activate();
    }
}
