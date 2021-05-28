using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public bool lockCursor;
    public float mouseHorizontalSensitivity = 4;
    public float mouseVerticalSensitivity = 4;
    public Vector3 offsetFromTarget = new Vector3(0, 1, 7.0f);
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


    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (target == null)
            target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotate();
        FollowTarget();
    }

    // 카메라 회전
    private void CameraRotate()
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
