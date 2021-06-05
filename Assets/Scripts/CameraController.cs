using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public bool lockCursor;
    public float mouseHorizontalSensitivity = 4;
    public float mouseVerticalSensitivity = 4;
    public Vector3 offsetFromTarget = new Vector3(0, 3.0f, 7.0f);
    public Vector2 pitchMinMax = new Vector2(-45, 85);

    public Transform target;

    // 부드러운 이동
    public bool smoothMovement = true;
    public float movementSmoothTime = 0.05f;
    private Vector3 movementSmoothVelocity;
    private Vector3 currentMovement;

    // 부드러운 회전
    public bool smoothRotation = true;
    public float rotationSmoothTime = 0.05f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;


    private float yaw;
    private float pitch;

    private Transform trans = null;

    [SerializeField] private bool isTargetPlayerCamera = true;
    [SerializeField] private bool isMainCamera = true;
    private Camera camera = null;

    [Header("0번은 메인, 1번은 플레이어 머리에 임시달아놨어요.")]
    [SerializeField] [Range(1,10)] private int cameraIdx = 1;

    public new Transform transform
    {
        get
        {
            if (trans == null)
            {
                trans = GetComponent<Transform>();
            }
            return trans;
        }
    }

    public bool IsTargetPlayerCamera { get => isTargetPlayerCamera; }
    public bool IsMainCamera { get => isMainCamera; }


    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        camera = GetComponent<Camera>();

        if (isMainCamera)
        {
            CameraManager.Instance.SetMainCamera(this);
        }
        else
        {
            CameraManager.Instance.AddCamera(cameraIdx, this);
        }

        if(IsTargetPlayerCamera)
        {
            target = PlayerManager.Instance.PlayerCtrl.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraCtrl();
    }

    public void CameraCtrl()
    {
        if (IsTargetPlayerCamera || isMainCamera)
            FollowTarget();

        if(isMainCamera)
            CameraRotate();
    }

    // 카메라 회전
    public void CameraRotate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseHorizontalSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseVerticalSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = smoothRotation ? currentRotation : targetRotation;
    }

    // 타겟 따라가기
    private void FollowTarget()
    {
        if (target == null)
            return;

        currentMovement =
            smoothMovement ?
            Vector3.SmoothDamp(currentMovement, target.position, ref movementSmoothVelocity, movementSmoothTime)
            : target.position;

        Vector3 newPos = currentMovement - transform.forward * offsetFromTarget.z + transform.right * offsetFromTarget.x;
        newPos.y += offsetFromTarget.y;
        transform.position = newPos;
    }
}
