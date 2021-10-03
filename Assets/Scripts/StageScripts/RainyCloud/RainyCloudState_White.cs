using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyCloudState_White : RainyCloudState
{

    public override void SetState(RainyCloud inCloud)
    {
        cloudType = CloudType.White;
        cloud = inCloud;


    }

    public override void Enter()
    {
        cloud.CloudMaterial.color = new Color(1f, 1f, 1f);
    }

    public override void Exit()
    {
    }


}
