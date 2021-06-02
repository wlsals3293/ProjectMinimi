using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerController : MonoBehaviour
{
    [Header("Default Status")]
    // Move
    [SerializeField] private float runSpeed = 10;
    [SerializeField] private float speedSmoothTime = 0.1f;
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
    [SerializeField] private float jumpPower = 18;
    private bool canJump = true;

    // air
    [SerializeField] private float gravity = -42f;


    private LayerMask steppableMask;

    private Rigidbody rb;
    private Transform cameraT;

    private Transform trans = null;
    public new Transform transform 
    { 
        get
        {
            if(trans == null)
            {
                trans = GetComponent<Transform>();
            }
            return trans;
        }
    }

    private PlayerCharacter playerCharacter = null;
    public PlayerCharacter PlayerCharacter { get => playerCharacter; }

    // input
    private Vector3 input;
    private Vector3 inputDir;
    private bool jumpInput;

    private FSMController fsm = new FSMController();

    private bool puase = false;
    public bool Puase 
    { 
        get
        {
            return puase;
        }
        set
        {
            if(value)
            {
                input = Vector3.zero;
            }

            rb.isKinematic = value;
            puase = value;
        }
    }
    

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        rb = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;

        steppableMask = LayerMask.GetMask("Ground", "Object", "Minimi");

        Idle_SetState();
        Holding_SetState();
    }

    public void Init()
    {
        ChangeState(PlayerState.Idle);
        Puase = false;
    }


    public void ChangeState(PlayerState state)
    {
        fsm.ChangeState(state);
    }

    private void Update()
    {
        // TODO 중력 생각 2가지 pause
        if (Puase == false)
        {
            fsm.Update();
        }
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
        bool isStepped = Physics.SphereCast(
            rb.position + (Vector3.up * 0.6f), 0.5f, Vector3.down, out hit,
            0.11f, steppableMask, QueryTriggerInteraction.Ignore);

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

    private string GetHitTag()
    {
        return null;
    }

    public void SetLocalPosition(Vector3 pos)
    {
        transform.position = pos;
    }


}