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

    // �ε巯�� �̵�
    public bool smoothMovement = true;
    public float movementSmoothTime = 0.05f;
    private Vector3 movementSmoothVelocity;
    private Vector3 currentMovement;

    // �ε巯�� ȸ��
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

    // ī�޶� ȸ��
    private void CameraRotate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseHorizontalSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseVerticalSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = smoothRotation ? currentRotation : targetRotation;
    }

    // Ÿ�� ���󰡱�
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
