using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold_Ctrl : Switch_C_OBJ
{
    public enum ScaffoldType {Reciprocate, Return}

    public ScaffoldType moveType;
    public Transform scaffold_trfm;
    public Rigidbody scaffold_rgd;
    public Transform start;
    public Transform end;
    bool isConnect = false;
    private bool goToStart = false;
    private bool goToEnd = true;
    public Vector3 direction;

    public float speed = 4.0f;

   
    // Start is called before the first frame update
    void Start()
    {
        scaffold_trfm.position = start.position; //�� ���� �� ���������� �����̵�

        if(switchs.Count > 0) //������ ����ġ�� �����ϸ� isConnect�� true��
        {
            isConnect = true;
        }

        if (isConnect) //������ ����ġ�� ������
        {
            ConnectingSwitch();
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
            if(moveType == ScaffoldType.Return) // ȸ����
            {
                ReturnMoving();
            }

            if(moveType == ScaffoldType.Reciprocate && AllSwitchOn) //�պ����̰� ����ġ�� ������������
            {
                ReciprocateMoving();
            }
        }
    }
    public void ReciprocateMoving() //�պ�������
    {
        if (goToEnd) //���������� ����������
        {
            direction = end.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
        }

        else //���������� ����������
        {
            direction = start.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
        }
            
        DirectionSwitching(); //�������̳� �������� �����ϸ� �ݴ��������� �ٽ� ���ϵ��� ���� ��Ʈ��
    }
    public void ReturnMoving() //ȸ���� ������
    {
        if (AllSwitchOn) //����� ����ġ�� ������
        {
            if (Vector3.Distance(scaffold_trfm.position, end.transform.position) > 0.3f) //���� �������� �������� �ʾ����� ����������
            {
                direction = end.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
            }
        }

        else //����� ����ġ�� ����������
        {
            if (Vector3.Distance(scaffold_trfm.position, start.transform.position) > 0.3f) //���� �������� �������� �ʾ����� ����������
            {
                direction = start.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
            }
        }
    }
    public void DirectionSwitching() //�������̳� �������� �����ϸ� �ݴ��������� �ٽ� ���ϵ��� ���� ��Ʈ��
    {
        
            if (goToEnd && Vector3.Distance(scaffold_trfm.position, end.transform.position) < 0.3f) //�������� ������ �ٽ� ���������� ���ϰ�
            {
                goToEnd = false;
                goToStart = true;
            }
            if (goToStart && Vector3.Distance(scaffold_trfm.position, start.transform.position) < 0.3f) //�������� ������ �ٽ� ���������� ���ϰ�
            {
                goToStart = false;
                goToEnd = true;
            }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Activate();
    }
}
