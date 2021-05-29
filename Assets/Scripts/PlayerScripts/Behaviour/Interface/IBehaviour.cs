using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBehaviour
{
    public void Enter(PlayerState prev);
    public void Update();
    public void Exit(PlayerState next);

}

public class SimpleBehaviour : IBehaviour
{
    private Action<PlayerState> enter = null;
    private Action update = null;
    private Action<PlayerState> exit = null;

    public SimpleBehaviour(Action<PlayerState> enter, Action update, Action<PlayerState> exit)
    {
        this.enter = enter;
        this.update = update;
        this.exit = exit;
    }

    public void Enter(PlayerState prev)
    {
        if(enter != null)
        {
            enter(prev);
        }
    }

    public void Exit(PlayerState next)
    {
        if (exit != null)
        {
            exit(next);
        }
    }

    public void Update()
    {
        if (update != null)
        {
            update();
        }
    }
}