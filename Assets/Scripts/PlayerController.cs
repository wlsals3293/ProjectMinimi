using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerAniState
{
    Idle = 0,
    Run = 1,
    Jump_Start = 2,
    Air = 3,
    Jump_End = 4,
    Dead = 5,
    Hold_Start = 6,
    Holding = 7,
    Hold_End = 8
}

public class PlayerController : MonoBehaviour
{
    // input
    Vector3 input;
    Vector3 inputDir;
    bool jumpInput;

    Vector3 moveDirection;


    // Move, Turn
    public float runSpeed = 10;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    //public float maxGroundAngle = 120;
    //public float maxStepHeight = 0.1f;


    bool isOnGround = true;

    float verticalVelocity = 0;
    Vector3 moveVelocity = Vector3.zero;


    // jump
    public float jumpPower = 18;
    bool canJump = true;


    // air
    public float gravity = -42f;


    LayerMask stepableMask;

    Rigidbody rb;
    Transform cameraT;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;

        stepableMask = LayerMask.GetMask("Ground", "Object");
    }

    private void Update()
    {
        GetInput();
        ApplyGravity();
        DetectGround();

        if (jumpInput && canJump)
        {
            Jump();
        }

        Move();
        Turn();

        rb.velocity = moveVelocity + (Vector3.up * verticalVelocity);
    }

    private void GetInput()
    {
        input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0.0f,
            Input.GetAxisRaw("Vertical")
            );
        inputDir = input.normalized;
        jumpInput = Input.GetButton("Jump");

        moveDirection = (input.z * Vector3.Scale(cameraT.forward, new Vector3(1, 0, 1)).normalized + input.x * cameraT.right).normalized;
    }

    private void Move()
    {
        float targetSpeed = input.sqrMagnitude > 0.01f ? runSpeed : 0.0f;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        moveVelocity = moveDirection * currentSpeed;
    }

    private void DetectGround()
    {
        RaycastHit hit;
        bool isStepped = Physics.SphereCast(rb.position + (Vector3.up * 0.6f), 0.5f, Vector3.down, out hit, 0.11f, stepableMask);

        if (isStepped)
        {
            if (verticalVelocity <= 0)
                verticalVelocity = 0;

            if (!isOnGround)
            {
                isOnGround = true;
                canJump = true;
            }
        }
        else if (!isStepped && isOnGround)
        {
            isOnGround = false;
        }
    }

    private void Turn()
    {
        if (input != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

            // 회전의 기본이 되는 코드
            //transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
        }
    }

    private void Jump()
    {
        verticalVelocity += jumpPower;
        canJump = false;
    }

    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }
}