using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold_Ctrl : Switch_C_OBJ
{
    public enum ScaffoldType { Reciprocate, Return }

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
    public float waitTime = 1.0f;

    private Coroutine waitCor;
    private bool alreadyWait = false;

    void Start()
    {
        scaffold_trfm.position = start.position; //씬 시작 시 시작점으로 순간이동

        if (switchs.Count > 0) //연결할 스위치가 존재하면 isConnect를 true로
        {
            isConnect = true;
        }

        if (isConnect) //연결할 스위치가 있으면
        {
            ConnectingSwitch();
        }
    }
    public void ScaffoldMove()
    {
        if (!isConnect) //연결된 스위치가 없으면 자동으로 왕복 반복
        {
            ReciprocateMoving();
        }

        else //연결된 스위치가 있다면 MoveType에 따라 움직임
        {
            if (moveType == ScaffoldType.Return) // 회귀형
            {
                ReturnMoving();
            }

            else if (moveType == ScaffoldType.Reciprocate && AllSwitchOn) //왕복형이고 스위치가 눌러져있으면
            {
                ReciprocateMoving();
            }
        }
    }
    public void ReciprocateMoving() //왕복움직임
    {
        if (goToEnd) //시작점에서 종착점으로
        {
            direction = end.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
        }

        else //종착점에서 시작점으로
        {
            direction = start.position - scaffold_trfm.position;
            scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
        }

        DirectionSwitching(); //시작점이나 종착점에 도착하면 반대지점으로 다시 향하도록 방향 컨트롤
    }
    public void ReturnMoving() //회귀형 움직임
    {
        if (AllSwitchOn) //연결된 스위치가 켜지면
        {
            if (Vector3.Distance(scaffold_trfm.position, end.transform.position) > 0.3f) //아직 종착점에 도착하지 않았으면 종착점으로
            {
                direction = end.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
            }
        }

        else //연결된 스위치가 꺼져있으면
        {
            if (Vector3.Distance(scaffold_trfm.position, start.transform.position) > 0.3f) //아직 시작점에 도착하지 않았으면 시작점으로
            {
                direction = start.position - scaffold_trfm.position;
                scaffold_rgd.MovePosition(scaffold_trfm.position + direction.normalized * speed * Time.deltaTime);
            }
        }
    }
    public void DirectionSwitching() //시작점이나 종착점에 도착하면 반대지점으로 다시 향하도록 방향 컨트롤
    {

        if (goToEnd && Vector3.Distance(scaffold_trfm.position, end.transform.position) < 0.1f) //종착점에 닿으면 다시 시작점으로 향하게
        {
            if (!alreadyWait)
            {
                alreadyWait = true;
                waitCor = StartCoroutine(Wait());
            }
        }
        if (goToStart && Vector3.Distance(scaffold_trfm.position, start.transform.position) < 0.1f) //시작점에 닿으면 다시 종착점으로 향하게
        {
            if (!alreadyWait)
            {
                alreadyWait = true;
                waitCor = StartCoroutine(Wait());
            }
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!alreadyWait)
        {
            ScaffoldMove();
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        goToEnd = !goToEnd;
        goToStart = !goToStart;
        alreadyWait = false;
        StopCoroutine(waitCor);
    }
}
