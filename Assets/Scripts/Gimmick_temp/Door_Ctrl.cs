using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door_Ctrl : Switch_C_OBJ
{
    public enum DoorType {Maintain, OnOff}
    public DoorType doorType;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool isOpen;

    public float doorSpd = 4.0f;
    void Start()
    { 
        startPoint = transform.position;
        endPoint = startPoint - new Vector3(0, transform.lossyScale.y, 0);

        ConnectingSwitch();
    }


    public void DoorOpen()
    {
        if (AllSwitchOn) isOpen = true; //����� ����ġ�� ���� ������ isOpen = true

        else if (!AllSwitchOn && doorType == DoorType.OnOff) isOpen = false; //����� ����ġ �� �ϳ��� �����ְ� ���� Ÿ���� OnOff���̸� isOpen = false

        if (isOpen) //isOpen�� true�� ����
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, doorSpd * Time.deltaTime); 
        }
        else //isOpen�� false�� ����
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint, doorSpd * Time.deltaTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        DoorOpen();
    }
}
