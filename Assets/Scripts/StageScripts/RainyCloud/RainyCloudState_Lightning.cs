using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyCloudState_Lightning : RainyCloudState
{
    public override void SetState(RainyCloud inCloud)
    {
        cloudType = CloudType.Lightning;
        cloud = inCloud;
    }

    public override void Enter()
    {
        cloud.CloudMaterial.color = new Color(0.2f, 0.2f, 0.2f);

    }

    public override void Exit()
    {
    }
}
