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
        scaffold_trfm.position = _start.position; //씬 시작 시 시작점으로 순간이동

        if(_switchs.Count > 0) //연결할 스위치가 존재하면 isConnect를 true로
        {
            isConnect = true;
        }

        s_Ctrl = GetComponent<Scaffold_Ctrl>(); //발판 스크립트

        if (isConnect) //연결할 스위치가 있으면
        {
            foreach (Switchs_Ctrl switchs in _switchs) //스위치들과 연결
            {
                switchs.Connecting(s_Ctrl); 
            }
        }
    }
    public override void Activate() //문이 열리는 행동 혹은 발판의 움직임
    {
        if (!isConnect) //연결된 스위치가 없으면 자동으로 왕복 반복
        {
            ReciprocateMoving();
        }

        else //연결된 스위치가 있다면 MoveType에 따라 움직임
        {
            if(MoveType == ScaffoldType.Return) // 회귀형
            {
                ReturnMoving();
            }

            if(MoveType == ScaffoldType.Reciprocate && AllSwitchOn) //왕복형이고 스위치가 눌러져있으면
            {
                ReciprocateMoving();
            }
        }
    }

    public void ReciprocateMoving() //왕복움직임
    {
        if (GoToEnd) //시작점에서 종착점으로
        {
            direction = _end.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
        }

        else //종착점에서 시작점으로
        {
            direction = _start.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
        }
            
        DirectionSwitching(); //시작점이나 종착점에 도착하면 반대지점으로 다시 향하도록 방향 컨트롤
    }

    public void ReturnMoving() //회귀형 움직임
    {
        if (AllSwitchOn) //연결된 스위치가 켜지면
        {
            if (Vector3.Distance(scaffold_trfm.position, _end.transform.position) > 0.3f) //아직 종착점에 도착하지 않았으면 종착점으로
            {
                direction = _end.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
            }
        }

        else //연결된 스위치가 꺼져있으면
        {
            if (Vector3.Distance(scaffold_trfm.position, _start.transform.position) > 0.3f) //아직 시작점에 도착하지 않았으면 시작점으로
            {
                direction = _start.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * Speed * Time.deltaTime);
            }
        }
    }

    public void DirectionSwitching() //시작점이나 종착점에 도착하면 반대지점으로 다시 향하도록 방향 컨트롤
    {
        
            if (GoToEnd && Vector3.Distance(scaffold_trfm.position, _end.transform.position) < 0.3f) //종착점에 닿으면 다시 시작점으로 향하게
            {
                GoToEnd = false;
                GoToStart = true;
            }
            if (GoToStart && Vector3.Distance(scaffold_trfm.position, _start.transform.position) < 0.3f) //시작점에 닿으면 다시 종착점으로 향하게
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
