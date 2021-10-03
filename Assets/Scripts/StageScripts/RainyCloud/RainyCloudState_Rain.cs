using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyCloudState_Rain : RainyCloudState
{
    public override void SetState(RainyCloud inCloud)
    {
        cloudType = CloudType.Rain;
        cloud = inCloud;
    }

    public override void Enter()
    {
        cloud.CloudMaterial.color = new Color(0.7f, 0.7f, 0.7f);
    }

    public override void Exit()
    {
    }
}
