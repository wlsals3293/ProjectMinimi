using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RainyCloudState
{
    public bool useUpdate = false;

    public bool useFixedUpdate = false;

    protected CloudType cloudType = CloudType.None;

    protected RainyCloud cloud;

    


    public virtual void SetState(RainyCloud inCloud)
    {
        cloud = inCloud;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }

}
