using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController
{
    private PlayerState curState = PlayerState.None;

    public PlayerState CurState { get => curState; }

    private SimpleBehaviour curBehaviour = null;


    public void Update()
    {
        if(curBehaviour != null)
        {
            curBehaviour.Update();
        }
    }

    public void ChangeState(PlayerState nextState)
    {
        if(curBehaviour != null)
        {
            curBehaviour.Exit(nextState);
        }

        // Update Stop
        curBehaviour = null;

        PlayerState prev = curState;
        curState = nextState;

        curBehaviour = PlayerManager.Instance.GetBehaviour(nextState);

        if (curBehaviour != null)
        {
            curBehaviour.Enter(prev);
        }
    }

}
