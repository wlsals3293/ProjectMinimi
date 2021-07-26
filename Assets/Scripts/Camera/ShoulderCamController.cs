using UnityEngine;

public class ShoulderCamController : CameraController
{
    [Tooltip("카메라가 따라가는 목표물")]
    [SerializeField]
    private Transform target;



    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            virtualCamera.Follow = value;
        }
    }




}
