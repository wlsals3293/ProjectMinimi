using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold_Ctrl : Switch_C_OBJ
{
    public enum ScaffoldType {Reciprocate, Return}

    public ScaffoldType MoveType;
    public Transform Scaffold;
    public Transform _start;
    public Transform _end;
    bool isConnect = false;
    bool SwitchOn = false;
    private bool GoToStart = false;
    private bool GoToEnd = true;
    private Scaffold_Ctrl s_Ctrl;

    public float Speed = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        Scaffold.position = _start.position;

        if(_switchs.Count > 0)
        {
            isConnect = true;
        }

        s_Ctrl = GetComponent<Scaffold_Ctrl>();

        if (_switchs.Count > 0)
        {
            foreach (Switchs_Ctrl switchs in _switchs)
            {
                switchs.Connecting(s_Ctrl);
            }
        }
    }
    public override void Activate() //문이 열리는 행동 혹은 발판의 움직임
    {
        if (!isConnect)
        {
            ReciprocateMoving();
        }

        else if (isConnect)
        {
            if(MoveType == ScaffoldType.Return)
            {
                ReturnMoving();
            }

            if(MoveType == ScaffoldType.Reciprocate && SwitchOn)
            {
                ReciprocateMoving();
            }
        }
    }

    public void ReciprocateMoving()
    {
        if (GoToEnd)
            Scaffold.position = Vector3.MoveTowards(Scaffold.position, _end.position, Speed * Time.deltaTime);

        if (GoToStart)
        {
            Scaffold.position = Vector3.MoveTowards(Scaffold.position, _start.position, Speed * Time.deltaTime);
        }

        DirectionSwitching();
    }

    public void ReturnMoving()
    {
        if (SwitchOn)
        {
            Scaffold.position = Vector3.MoveTowards(Scaffold.position, _end.position, Speed * Time.deltaTime);
        }

        if (!SwitchOn)
        {
            Scaffold.position = Vector3.MoveTowards(Scaffold.position, _start.position, Speed * Time.deltaTime);
        }
    }

    public void DirectionSwitching()
    {
        if(GoToEnd && Scaffold.position == _end.position)
        {
            GoToEnd = false;
            GoToStart = true;
        }
        if (GoToStart && Scaffold.position == _start.position)
        {
            GoToStart = false;
            GoToEnd = true;
        }
    }


    public override void SwitchCheck()
    {
        int i = 0;
        foreach (Switchs_Ctrl switchs in _switchs)
        {
            if (switchs.isActivate)
            {
                i++;
                Debug.Log("dd");
            }
        }

        if (i >= _switchs.Count)
        {
            SwitchOn = true;
        }

        if (i < _switchs.Count)
        {
            SwitchOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Activate();
    }
}
