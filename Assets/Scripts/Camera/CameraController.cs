using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Config")]

    [SerializeField, Range(0, 10)]
    protected int cameraIdx = 0;

    

    /// <summary>
    /// ���� ȸ�� ����. x�� ȸ��
    /// </summary>
    [SerializeField, ReadOnly]
    protected float pitch;

    /// <summary>
    /// �¿� ȸ�� ����. y�� ȸ��
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
            Debug.LogWarning("VirtualCamera�� �������� �ʽ��ϴ�.");

        CameraManager.Instance.AddCustomCamera(cameraIdx, this);
    }

    /// <summary>
    /// ī�޶� Ȱ��ȭ �Ǿ��� �� �� ������ ����
    /// </summary>
    public virtual void CameraUpdate()
    {

    }

    /// <summary>
    /// ī�޶� ȸ��
    /// </summary>
    public virtual void Rotate(float deltaX, float deltaY)
    {
        
    }


    /// <summary>
    /// ���� ī�޶��� ȸ������ �޾� �����մϴ�
    /// </summary>
    /// <param name="rotation"></param>
    public virtual void ApplyPreMovement(Quaternion rotation)
    {

    }

    /// <summary>
    /// ī�޶��� ���� ��ġ�� ȸ������ ���� Ʈ�������� ����
    /// </summary>
    protected virtual void ApplyFinalChanges()
    {
    }
}
