using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Config")]

    [SerializeField, Range(0, 10)]
    protected int cameraIdx = 0;

    

    /// <summary>
    /// 상하 회전 각도. x축 회전
    /// </summary>
    [SerializeField, ReadOnly]
    protected float pitch;

    /// <summary>
    /// 좌우 회전 각도. y축 회전
    /// </summary>
    [SerializeField, ReadOnly]
    protected float yaw;



    protected CinemachineVirtualCamera virtualCamera;



    public int CameraIdx { get => cameraIdx; }

    public CinemachineVirtualCamera VCam
    {
        get => virtualCamera;
    }

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

        CameraManager.Instance.AddCustomCamera(cameraIdx, this);
    }

    /// <summary>
    /// 카메라가 활성화 되었을 때 매 프레임 실행
    /// </summary>
    public virtual void CameraUpdate()
    {

    }

    /// <summary>
    /// 카메라 회전
    /// </summary>
    public virtual void Rotate(float deltaX, float deltaY)
    {
        
    }


    /// <summary>
    /// 이전 카메라의 회전값을 받아 적용합니다
    /// </summary>
    /// <param name="rotation"></param>
    public virtual void ApplyPreMovement(Quaternion rotation)
    {

    }

    /// <summary>
    /// 카메라의 최종 위치와 회전값을 실제 트랜스폼에 적용
    /// </summary>
    protected virtual void ApplyFinalChanges()
    {
    }
}
