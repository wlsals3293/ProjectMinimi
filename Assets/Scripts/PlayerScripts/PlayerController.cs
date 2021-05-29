using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerAniState
{
    Idle,
    Run,
    Jump_Start,
    Air,
    Jump_End,
    Dead,
    Hold_Start,
    Holding,
    Hold_End
}

public class PlayerController : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour = null;
    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; }

    private PlayerCharacter playerCharacter = null;

    public PlayerCharacter PlayerCharacter { get => playerCharacter; }

    // input
    private Vector3 input;
    private Vector3 inputDir;
    private bool jumpInput;

    // Move
    public float runSpeed = 10;
    public float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;
    private float currentSpeed;
    //public float maxGroundAngle = 120;
    //public float maxStepHeight = 0.1f;
    private bool isOnGround = true;
    private float verticalVelocity = 0;
    private Vector3 moveDirection;
    private Vector3 moveVelocity = Vector3.zero;

    // Turn
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // jump
    public float jumpPower = 18;
    private bool canJump = true;

    // air
    public float gravity = -42f;


    private LayerMask steppableMask;

    private Rigidbody rb;
    private Transform cameraT;

   

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        playerBehaviour = new PlayerBehaviour(transform);
        rb = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;

        steppableMask = LayerMask.GetMask("Ground", "Object");
    }

    private void Update()
    {
#if UNITY_EDITOR
        playerBehaviour.DrawLineRaycatAllways();
#endif

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
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerBehaviour.UpdateActiveKeyAction(ActiveKeyType.Use);
        }


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
        bool isStepped = Physics.SphereCast(rb.position + (Vector3.up * 0.6f), 0.5f, Vector3.down, out hit, 0.11f, steppableMask);

        if (isStepped && verticalVelocity <= 0.0f)
        {
            verticalVelocity = 0.0f;

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
        verticalVelocity = jumpPower;
        canJump = false;
    }

    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }
}