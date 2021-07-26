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
            Debug.LogWarning("VirtualCamera�� �������� �ʽ��ϴ�.");

    }

    protected void Start()
    {
        CameraManager.Instance.AddCustomCamera(cameraIdx, this);

        if (cameraIdx == 0)
            CameraManager.Instance.ActivateCustomCamera(0);
    }

    /// <summary>
    /// ī�޶� Ȱ��ȭ �Ǿ��� �� �� ������ ����
    /// </summary>
    public virtual void CameraUpdate()
    {

    }


    public virtual void ApplyPreMovement(Quaternion rotation)
    {

    }


}
