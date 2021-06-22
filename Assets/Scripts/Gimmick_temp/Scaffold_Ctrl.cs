using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold_Ctrl : Switch_C_OBJ
{
    public enum ScaffoldType {Reciprocate, Return}

    public ScaffoldType MoveType;
    public Transform scaffold_trfm;
    public Rigidbody scaffold_rgd;
    public Transform _start;
    public Transform _end;
    bool isConnect = false;
    private bool GoToStart = false;
    private bool GoToEnd = true;
    private Scaffold_Ctrl s_Ctrl;
    public Vector3 direction;

    public float Speed = 4.0f;

   
    // Start is called before the first frame update
    void Start()
    {
        scaffold_trfm.position = _start.position; //�� ���� �� ���������� �����̵�

        if(_switchs.Count > 0) //������ ����ġ�� �����ϸ� isConnect�� true��
        {
            isConnect = true;
        }

        s_Ctrl = GetComponent<Scaffold_Ctrl>(); //���� ��ũ��Ʈ

        if (isConnect) //������ ����ġ�� ������
        {
            foreach (Switchs_Ctrl switchs in _switchs) //����ġ��� ����
            {
                switchs.Connecting(s_Ctrl); 
            }
        }
    }
    public override void Activate() //���� ������ �ൿ Ȥ�� ������ ������
    {
        if (!isConnect) //����� ����ġ�� ������ �ڵ����� �պ� �ݺ�
        {
            ReciprocateMoving();
        }

        else //����� ����ġ�� �ִٸ� MoveType�� ���� ������
        {
            if(MoveType == ScaffoldType.Return) // ȸ����
            {
                ReturnMoving();
            }

            if(MoveType == ScaffoldType.Reciprocate && AllSwitchOn) //�պ����̰� ����ġ�� ������������
            {
                ReciprocateMoving();
            }
        }
    }

    public void ReciprocateMoving() //�պ�������
    {
        if (GoToEnd) //���������� ����������
        {
            direction = _end.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
        }

        else //���������� ����������
        {
            direction = _start.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
        }
            
        DirectionSwitching(); //�������̳� �������� �����ϸ� �ݴ��������� �ٽ� ���ϵ��� ���� ��Ʈ��
    }

    public void ReturnMoving() //ȸ���� ������
    {
        if (AllSwitchOn) //����� ����ġ�� ������
        {
            if (Vector3.Distance(scaffold_trfm.position, _end.transform.position) > 0.3f) //���� �������� �������� �ʾ����� ����������
            {
                direction = _end.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
            }
        }

        else //����� ����ġ�� ����������
        {
            if (Vector3.Distance(scaffold_trfm.position, _start.transform.position) > 0.3f) //���� �������� �������� �ʾ����� ����������
            {
                direction = _start.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
            }
        }
    }

    public void DirectionSwitching() //�������̳� �������� �����ϸ� �ݴ��������� �ٽ� ���ϵ��� ���� ��Ʈ��
    {
        
            if (GoToEnd && Vector3.Distance(scaffold_trfm.position, _end.transform.position) < 0.3f) //�������� ������ �ٽ� ���������� ���ϰ�
            {
                GoToEnd = false;
                GoToStart = true;
            }
            if (GoToStart && Vector3.Distance(scaffold_trfm.position, _start.transform.position) < 0.3f) //�������� ������ �ٽ� ���������� ���ϰ�
            {
                GoToStart = false;
                GoToEnd = true;
            }
        
    }


   

    // Update is called once per frame
    void FixedUpdate()
    {
        Activate();
    }
}
