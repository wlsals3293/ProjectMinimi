using UnityEngine;

public class FreeLookCamController : CameraController
{
    [Header("FreeLook")]

    [Tooltip("ī�޶� ���󰡴� ��ǥ��")]
    [SerializeField]
    protected Transform target;

    [Tooltip("��ǥ�������κ��� ī�޶������ �Ÿ�")]
    public float cameraDistance = 3.5f;

    [Tooltip("ī�޶� ����ȸ�� �ּ�, �ִ밢��")]
    [SerializeField]
    private AngleLimit pitchLimit = new AngleLimit(-70.0f, 89.9f);



    [Header("Follow")]


    [Tooltip("��ǥ���� ��ġ ������ (�������)")]
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0.0f);

    /// <summary>
    /// ��ġ �������� ī�޶��� �������� ����� ��ġ
    /// </summary>
    private Vector3 targetPosition;


    [Tooltip("�ε巯�� �̵� ���� ����")]
    [SerializeField]
    private bool smoothMovement = true;

    [Tooltip("�����̵� ������. ���� Ŭ���� ��ǥ�� �ʰ� ����")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float horizontalDamping = 0.05f;

    [Tooltip("�����̵� ������. ���� Ŭ���� ��ǥ�� �ʰ� ����")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float verticalDamping = 0.3f;

    [Tooltip("�����̵� ������. ������ǥ ����")]
    [SerializeField]
    private float deadZoneHorizontal = 2.5f;

    [Tooltip("�����̵� ������. ������ǥ ����")]
    [SerializeField]
    private float deadZoneVertical = 1.5f;

    /// <summary>
    /// �����̵� ������ ������
    /// </summary>
    private float deadZoneVerticalOffset = -0.5f;



    private float xDampVelocity;
    private float yDampVelocity;
    private float zDampVelocity;



    [Header("Orbit")]

    [Tooltip("����� �Է��� ���� ī�޶� ȸ���� Ȱ��ȭ�մϴ�")]
    [SerializeField]
    private bool useRotation = true;


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
    [SerializeField, Range(0.0f, 0.1f)]
    private float rotationDamping = 0.05f;


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

        if (pitch > 180.0f) pitch -= 360.0f;
        else if (pitch < -180.0f) pitch += 360.0f;

        targetEulerAngles.x = pitch;
        targetEulerAngles.y = yaw;
    }

    /// <summary>
    /// ī�޶� ȸ��
    /// </summary>
    public override void Rotate(float deltaX, float deltaY)
    {
        if (!useRotation)
            return;

        yaw += deltaX;
        pitch -= deltaY;

        if (yaw < -180.0f)
            yaw += 360.0f;
        else if (yaw > 180.0f)
            yaw -= 360.0f;

        pitch = Mathf.Clamp(pitch, pitchLimit.min, pitchLimit.max);

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


            // ������ ����
            float deadZoneV = targetPosition.y > adjustedPosition.y ?
                Mathf.Min(targetPosition.y, adjustedPosition.y + deadZoneVertical + deadZoneVerticalOffset) :
                Mathf.Max(targetPosition.y, adjustedPosition.y - deadZoneVertical + deadZoneVerticalOffset);

            Vector3 deadZoneH = targetPosition - adjustedPosition;
            deadZoneH.y = 0.0f;

            if(deadZoneH.magnitude > deadZoneHorizontal)
            {
                targetPosition = adjustedPosition + (deadZoneH.normalized * deadZoneHorizontal);
            }
            targetPosition.y = deadZoneV;
        }
        else
        {
            targetPosition = adjustedPosition;
        }
    }

    private void AvoidObstacle()
    {

    }

    /// <summary>
    /// ī�޶��� ���� ��ġ�� ȸ������ ���� Ʈ�������� ����
    /// </summary>
    protected override void ApplyFinalChanges()
    {
        Vector3 cameraDistanceVector = currentRotation * -Vector3.forward * cameraDistance;
        Vector3 finalPosition = targetPosition + cameraDistanceVector;

        transform.SetPositionAndRotation(finalPosition, currentRotation);
    }
}
