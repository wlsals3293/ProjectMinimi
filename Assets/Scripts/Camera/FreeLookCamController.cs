using UnityEngine;

public class FreeLookCamController : CameraController
{
    [Header("FreeLook")]

    [Tooltip("카메라가 따라가는 목표물")]
    [SerializeField]
    protected Transform target;


    [Tooltip("목표지점으로부터 카메라까지의 거리")]
    public float cameraDistance = 6.0f;

    

    [Tooltip("카메라 상하회전 최소, 최대값")]
    public Vector2 pitchMinMax = new Vector2(-70, 89);



    [Header("Follow")]


    [Tooltip("목표물의 위치 보정값 (월드기준)")]
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0.0f);

    /// <summary>
    /// 위치 보정값과 카메라의 움직임이 적용된 위치
    /// </summary>
    private Vector3 targetPosition;


    [Tooltip("부드러운 이동 적용 여부")]
    public bool smoothMovement = true;

    [Tooltip("수평이동 지연값. 값이 클수록 목표를 늦게 따라감")]
    [Range(0.0f, 3.0f)]
    public float horizontalDamping = 0.05f;

    [Tooltip("수직이동 지연값. 값이 클수록 목표를 늦게 따라감")]
    [Range(0.0f, 3.0f)]
    public float verticalDamping = 0.2f;


    private float xDampVelocity;
    private float yDampVelocity;
    private float zDampVelocity;



    [Header("Orbit")]


    [SerializeField] private bool useRotation = true;


    /// <summary>
    /// 카메라의 현재 회전값
    /// </summary>
    private Quaternion currentRotation;

    /// <summary>
    /// 카메라의 현재 회전 각도
    /// </summary>
    private Vector3 targetEulerAngles;


    [Tooltip("부드러운 회전 적용 여부")]
    public bool smoothRotation = true;

    [Tooltip("회전 지연값. 값이 클수록 늦게 회전함")]
    [Range(0.0f, 3.0f)]
    public float rotationDamping = 0.05f;


    private float pitchDampVelocity;
    private float yawDampVelocity;



    public Transform Target { get => target; set => target = value; }

    public bool UseRotation { get => useRotation; set => useRotation = value; }



    /// <summary>
    /// 카메라가 활성화 되었을 때 매 프레임 실행
    /// </summary>
    public override void CameraUpdate()
    {
        FollowTarget();

        ApplyFinalChanges();
    }

    /// <summary>
    /// 이전 카메라의 회전값을 받아 적용합니다
    /// </summary>
    /// <param name="rotation"></param>
    public override void ApplyPreMovement(Quaternion rotation)
    {
        // 즉시 이동
        bool option = smoothMovement;
        smoothMovement = false;
        FollowTarget();
        smoothMovement = option;

        // 회전값 적용
        pitch = rotation.eulerAngles.x;
        yaw = rotation.eulerAngles.y;

        if (pitch > 180.0f) pitch -= 360.0f;
        else if (pitch < -180.0f) pitch += 360.0f;

        targetEulerAngles.x = pitch;
        targetEulerAngles.y = yaw;
    }

    /// <summary>
    /// 카메라 회전
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
    /// 목표물 따라가기
    /// </summary>
    private void FollowTarget()
    {
        if (target == null)
            return;

        // 오프셋 적용 (월드 기준)
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
    /// 카메라의 최종 위치와 회전값을 실제 트랜스폼에 적용
    /// </summary>
    protected override void ApplyFinalChanges()
    {
        Vector3 cameraDistanceVector = currentRotation * -Vector3.forward * cameraDistance;
        Vector3 finalPosition = targetPosition + cameraDistanceVector;

        transform.SetPositionAndRotation(finalPosition, currentRotation);
    }
}
