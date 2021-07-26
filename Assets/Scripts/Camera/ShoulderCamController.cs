using UnityEngine;

public class ShoulderCamController : CameraController
{
    [Tooltip("ī�޶� ���󰡴� ��ǥ��")]
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
