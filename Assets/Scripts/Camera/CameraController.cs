using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private int cameraIdx = 0;

    protected CinemachineVirtualCamera virtualCamera;



    public int CameraIdx { get => cameraIdx; }

    public int Priority
    {
        get => virtualCamera.Priority;
        set => virtualCamera.Priority = value;
    }


    protected void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera == null)
            Debug.LogWarning("VirtualCamera가 존재하지 않습니다.");

    }

    protected void Start()
    {
        CameraManager.Instance.AddCustomCamera(cameraIdx, this);

        if (cameraIdx == 0)
            CameraManager.Instance.ActivateCustomCamera(0);
    }

    /// <summary>
    /// 카메라가 활성화 되었을 때 매 프레임 실행
    /// </summary>
    public virtual void CameraUpdate()
    {

    }


    public virtual void ApplyPreMovement(Quaternion rotation)
    {

    }


}
