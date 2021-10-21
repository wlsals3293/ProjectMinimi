using UnityEngine;


public class FreeLookCamController : CameraController
{
    [Header("FreeLook")]

    [Tooltip("카메라가 따라가는 목표물")]
    [SerializeField]
    protected Transform target;

    [Tooltip("목표지점으로부터 카메라까지의 거리")]
    public float cameraDistance = 3.5f;

    [Tooltip("카메라 상하회전 최소, 최대각도")]
    [SerializeField]
    private AngleLimit pitchLimit = new AngleLimit(-70.0f, 89.9f);



    [Header("Follow")]


    [Tooltip("목표물의 위치 보정값 (월드기준)")]
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0.0f);

    /// <summary>
    /// 위치 보정값과 카메라의 움직임이 적용된 위치
    /// </summary>
    private Vector3 targetPosition;


    [Tooltip("부드러운 이동 적용 여부")]
    [SerializeField]
    private bool smoothMovement = true;

    [Tooltip("수평이동 지연값. 값이 클수록 목표를 늦게 따라감")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float horizontalDamping = 0.05f;

    [Tooltip("수직이동 지연값. 값이 클수록 목표를 늦게 따라감")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float verticalDamping = 0.2f;

    [Tooltip("수평이동 데드존. 월드좌표 기준")]
    [SerializeField]
    private float deadZoneHorizontal = 2.5f;

    [Tooltip("수직이동 데드존. 월드좌표 기준")]
    [SerializeField]
    private float deadZoneVertical = 1.5f;

    /// <summary>
    /// 수직이동 데드존 오프셋
    /// </summary>
    private float deadZoneVerticalOffset = -0.7f;



    private float xDampVelocity;
    private float yDampVelocity;
    private float zDampVelocity;



    [Header("Orbit")]

    [Tooltip("사용자 입력을 통한 카메라 회전을 활성화합니다")]
    [SerializeField]
    private bool useRotation = true;


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
    [SerializeField, Range(0.0f, 0.1f)]
    private float rotationDamping = 0.05f;


    private float pitchDampVelocity;
    private float yawDampVelocity;



    [Header("Collision")]

    [Tooltip("충돌체크할 레이어")]
    [SerializeField]
    private LayerMask collisionMask = 1;

    [Tooltip("카메라의 반경")]
    [SerializeField]
    private float collisionRadius = 0.2f;

    [Tooltip("카메라가 최대로 가까워질 수 있는 거리")]
    [SerializeField]
    private float minDistance = 1f;



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
    /// 카메라가 변경될 때 이전 카메라의 회전값을 받아 적용합니다
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

        currentRotation.eulerAngles = targetEulerAngles;
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


            // 데드존 적용
            float deadZoneV = targetPosition.y > adjustedPosition.y ?
                Mathf.Min(targetPosition.y, adjustedPosition.y + deadZoneVertical + deadZoneVerticalOffset) :
                Mathf.Max(targetPosition.y, adjustedPosition.y - deadZoneVertical + deadZoneVerticalOffset);

            Vector3 deadZoneH = targetPosition - adjustedPosition;
            deadZoneH.y = 0.0f;

            if (deadZoneH.magnitude > deadZoneHorizontal)
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

    /// <summary>
    /// 카메라가 충돌하는 거리를 구합니다.
    /// </summary>
    /// <param name="distance">카메라가 목표물로부터 떨어진 거리</param>
    /// <returns>카메라가 충돌한 거리</returns>
    private float GetCollisionDistance(float distance)
    {
        Ray ray = new Ray(targetPosition, currentRotation * -Vector3.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, collisionRadius, out hit, distance, collisionMask))
        {
            return Mathf.Max(hit.distance, minDistance);
        }

        return distance;
    }

    /// <summary>
    /// 카메라의 최종 위치와 회전값을 실제 트랜스폼에 적용
    /// </summary>
    protected override void ApplyFinalChanges()
    {
        Vector3 cameraDistanceVector =
            currentRotation * -Vector3.forward * GetCollisionDistance(cameraDistance);
        Vector3 finalPosition = targetPosition + cameraDistanceVector;

        transform.SetPositionAndRotation(finalPosition, currentRotation);
    }
}
