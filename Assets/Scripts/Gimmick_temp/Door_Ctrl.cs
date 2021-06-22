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
            if(switchs._connetObj != null) { Debug.LogError("�Ѱ��� ����ġ�� �Ѱ��� ������Ʈ�� ������ �����մϴ�."); }
            switchs.Connecting(thisDoor);
        }
    }


    public override void Activate()
    {
        if (AllSwitchOn) isOpen = true; //����� ����ġ�� ���� ������ isOpen = true

        else if (!AllSwitchOn && doorType == DoorType.OnOff) isOpen = false; //����� ����ġ �� �ϳ��� �����ְ� ���� Ÿ���� OnOff���̸� isOpen = false

        if (isOpen) //isOpen�� true�� ����
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPotint, DoorSpd * Time.deltaTime); 
        }
        else //isOpen�� false�� ����
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
