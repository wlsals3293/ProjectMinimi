using UnityEngine;

public class FreeLookCamController : CameraController
{
    [Header("Config")]


    [Tooltip("ī�޶� ���󰡴� ��ǥ��")]
    [SerializeField]
    private Transform target;

    [Tooltip("��ǥ�������κ��� ī�޶������ �Ÿ�")]
    public float cameraDistance = 10.0f;

    [Tooltip("���콺 ���� ����")]
    public float mouseHorizontalSensitivity = 4;

    [Tooltip("���콺 ���� ����")]
    public float mouseVerticalSensitivity = 4;



    [Header("Follow")]


    [Tooltip("��ǥ���� ��ġ ������ (�������)")]
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0.0f);

    /// <summary>
    /// ��ġ �������� ī�޶��� �������� ����� ��ġ
    /// </summary>
    private Vector3 targetPosition;


    [Tooltip("�ε巯�� �̵� ���� ����")]
    public bool smoothMovement = true;

    [Tooltip("�����̵� ������. ���� Ŭ���� ��ǥ�� �ʰ� ����")]
    [Range(0.0f, 3.0f)]
    public float horizontalDamping = 0.05f;

    [Tooltip("�����̵� ������. ���� Ŭ���� ��ǥ�� �ʰ� ����")]
    [Range(0.0f, 3.0f)]
    public float verticalDamping = 0.2f;


    private float xDampVelocity;
    private float yDampVelocity;
    private float zDampVelocity;



    [Header("Orbit")]


    [SerializeField] private bool useRotation = true;

    [Tooltip("ī�޶� ����ȸ�� �ּ�, �ִ밪")]
    public Vector2 pitchMinMax = new Vector2(-45, 85);

    /// <summary>
    /// ���� ȸ�� ����. x�� ȸ��
    /// </summary>
    private float pitch;

    /// <summary>
    /// �¿� ȸ�� ����. y�� ȸ��
    /// </summary>
    private float yaw;

    /// <summary>
    /// ī�޶��� ���� ȸ����
    /// </summary>
    private Quaternion currentRotation;

    /// <summary>
    /// ī�޶��� ���� ȸ�� ����
    /// </summary>
    private Vector3 targetEulerAngles;


    [Tooltip("�ε巯�� ȸ�� ���� ����")]
    public bool smoothRotation = true;

    [Tooltip("ȸ�� ������. ���� Ŭ���� �ʰ� ȸ����")]
    [Range(0.0f, 3.0f)]
    public float rotationDamping = 0.05f;


    private float pitchDampVelocity;
    private float yawDampVelocity;



    public Transform Target { get => target; set => target = value; }

    public bool UseRotation { get => useRotation; set => useRotation = value; }



    /// <summary>
    /// ī�޶� Ȱ��ȭ �Ǿ��� �� �� ������ ����
    /// </summary>
    public override void CameraUpdate()
    {
        FollowTarget();

        if (UseRotation)
            Rotate();

        ApplyFinalChanges();
    }

    /// <summary>
    /// ���� ī�޶��� ȸ������ �޾� �����մϴ�
    /// </summary>
    /// <param name="rotation"></param>
    public override void ApplyPreMovement(Quaternion rotation)
    {
        // ��� �̵�
        bool option = smoothMovement;
        smoothMovement = false;
        FollowTarget();
        smoothMovement = option;

        // ȸ���� ����
        pitch = rotation.eulerAngles.x;
        yaw = rotation.eulerAngles.y;
        targetEulerAngles.x = pitch;
        targetEulerAngles.y = yaw;
    }

    /// <summary>
    /// ī�޶� ȸ��
    /// </summary>
    public void Rotate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseHorizontalSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseVerticalSensitivity;

        if (yaw < -180.0f)
            yaw += 360.0f;
        else if (yaw > 180.0f)
            yaw -= 360.0f;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);


        if (smoothRotation)
        {
            targetEulerAngles.x = Mathf.SmoothDampAngle(
                targetEulerAngles.x, pitch, ref pitchDampVelocity, rotationDamping);
            targetEulerAngles.y = Mathf.SmoothDampAngle(
                targetEulerAngles.y, yaw, ref yawDampVelocity, rotationDamping);
        }
        else
        {
            targetEulerAngles.x = pitch;
            targetEulerAngles.y = yaw;
        }

        currentRotation.eulerAngles = targetEulerAngles;
    }

    /// <summary>
    /// ��ǥ�� ���󰡱�
    /// </summary>
    private void FollowTarget()
    {
        if (target == null)
            return;

        // ������ ���� (���� ����)
        Vector3 adjustedPosition = target.position + targetOffset;

        if (smoothMovement)
        {
            targetPosition.x = Mathf.SmoothDamp(
                targetPosition.x, adjustedPosition.x, ref xDampVelocity, horizontalDamping);
            targetPosition.y = Mathf.SmoothDamp(
                targetPosition.y, adjustedPosition.y, ref yDampVelocity, verticalDamping);
            targetPosition.z = Mathf.SmoothDamp(
                targetPosition.z, adjustedPosition.z, ref zDampVelocity, horizontalDamping);
        }
        else
        {
            targetPosition = adjustedPosition;
        }
    }

    /// <summary>
    /// ī�޶��� ���� ��ġ�� ȸ������ ���� Ʈ�������� ����
    /// </summary>
    private void ApplyFinalChanges()
    {
        Vector3 cameraDistanceVector = currentRotation * -Vector3.forward * cameraDistance;
        Vector3 finalPosition = targetPosition + cameraDistanceVector;

        transform.SetPositionAndRotation(finalPosition, currentRotation);
    }
}
