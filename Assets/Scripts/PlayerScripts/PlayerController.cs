using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;


public struct KeyInfo
{
    public bool current;
    public bool pre;
    public bool down;
    public bool up;

    public void Put(bool newInput)
    {
        pre = current;
        current = newInput;
        down = !pre && current;
        up = pre && !current;
    }
}

public partial class PlayerController : BaseCharacterController
{
    private const float RAY_DISTANCE = 2f;



    [Header("Player Controller")]

    [Tooltip("�ǰ��̻� ���ӽð�")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float hitDisorderTime = 0.6f;


    private Vector3 moveDirectionRaw;

    private Vector2 rotationInput;

    /// <summary>
    /// �÷��̾ �� ���̺��� �������� �����
    /// </summary>
    private float killY;

    [Tooltip("���콺 ���� ����")]
    public float mouseHorizontalSensitivity = 3;

    [Tooltip("���콺 ���� ����")]
    public float mouseVerticalSensitivity = 3;


    [SerializeField]
    private AnimationEventListener animEventListener = null;



    private KeyInfo mainAbilityAction1;


    private KeyInfo mainAbilityAction2;


    private bool key_alpha1, key_alpha2, key_alpha3;    // 1, 2, 3
    private bool key_interact;  // E
    private bool key_f;         // F


    private bool rotationChanging;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float elapsedChangingTime;
    private float changingTime;

    private bool hitDisordering;
    private float elapsedHitDisorderTime;

    private bool controlBlock = false;
    private float controlBlockTime;
    private float elapsedControlBlockTime;



    private bool noclipEnable = false;



    private AnimationMovement animMovement;


    private CapsuleCollider col;

    private PlayerAbility playerAbility = null;




    public delegate void TwoAxisDelegate(float deltaX, float deltaY);

    public TwoAxisDelegate onRotationAxisInput;


    private bool RotationChanging
    {
        get
        {
            return rotationChanging;
        }
        set
        {
            rotationChanging = value;
        }
    }

    private Rigidbody CachedRigidbody
    {
        get => movement.cachedRigidbody;
    }

    private Transform cameraT;

    private Transform CameraT
    {
        get
        {
            if (cameraT == null)
            {
                cameraT = CameraManager.Instance.MainCam.transform;
            }
            return cameraT;
        }
    }

    private Transform trans = null;
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

    private PlayerCharacter playerCharacter = null;
    public PlayerCharacter PlayerCharacter { get => playerCharacter; }

    private FSMController fsm = new FSMController();




    public override void Awake()
    {
        base.Awake();

        animator = GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
        playerCharacter = GetComponent<PlayerCharacter>();
        playerAbility = GetComponent<PlayerAbility>();
        animMovement = new AnimationMovement(CachedRigidbody);

        Idle_SetState();
        Hold_SetState();
        Dead_SetState();
        Climb_SetState();
        Sliding_SetState();
        Aim_SetState();
        Drag_SetState();
        LedgeGrab_SetState();

        StartCoroutine(RandomIdle());
    }

    public void Init()
    {
        killY = StageManager.Instance.GlobalKillY;
        ChangeState(PlayerState.Idle);
        pause = false;

        RegistEvents();
    }

    public void ChangeState(PlayerState state)
    {
        fsm.ChangeState(state);
    }

    public override void Update()
    {
        HandleInput();

        // TODO �߷� ���� 2���� pause
        if (isPaused)
            return;

        fsm.Update();

        if (fsm.CurState != PlayerState.None && fsm.CurState != PlayerState.Dead)
        {
            if (movement.cachedRigidbody.position.y < killY)
            {
                ChangeState(PlayerState.Dead);
            }
        }
    }

    protected override void FixedUpdate()
    {
        Pause();

        if (isPaused)
            return;

        fsm.FixedUpdate();
    }

    protected override void HandleInput()
    {
        // Toggle pause / resume.
        // By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ToggleESCMenu();
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            DebugController.Instance.OnToggleDebug();
        }


        // ���콺 ������ �Է�
        rotationInput = new Vector2
        {
            x = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity,
            y = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity
        };

        if (onRotationAxisInput != null)
            onRotationAxisInput(rotationInput.x, rotationInput.y);


        // ���� �Ұ����� ���¸� ����
        if (controlBlock)
            return;

        // Handle user input
        moveDirectionRaw = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0.0f,
            z = Input.GetAxisRaw("Vertical")
        };
        moveDirection = moveDirectionRaw;

        // �ֱ�� (���콺 ��Ŭ��, ����Ŭ��)
        mainAbilityAction1.Put(Input.GetMouseButton(0));
        mainAbilityAction2.Put(Input.GetMouseButton(1));

        jump = Input.GetButton("Jump");
        key_interact = Input.GetKeyDown(KeyCode.E);
        key_f = Input.GetKeyDown(KeyCode.F);
        key_alpha1 = Input.GetKeyDown(KeyCode.Alpha1);

    }


    /// <summary>
    /// Calculate the desired movement velocity.
    /// Eg: Convert the input (moveDirection) to movement velocity vector,
    ///     use navmesh agent desired velocity, etc.
    /// </summary>
    protected override Vector3 CalcDesiredVelocity()
    {
        // If using root motion and root motion is being applied (eg: grounded),
        // use animation velocity as animation takes full control

        if (useRootMotion && applyRootMotion)
            return rootMotionController.animVelocity;

        // else, convert input (moveDirection) to velocity vector

        return moveDirection * speed;
    }

    /// <summary>
    /// Perform character movement logic.
    /// 
    /// NOTE: Must be called in FixedUpdate.
    /// </summary>
    protected override void Move()
    {
        if (noclipEnable)
        {
            NoclipMove();
            return;
        }

        // Apply movement

        // If using root motion and root motion is being applied (eg: grounded),
        // move without acceleration / deceleration, let the animation takes full control

        var desiredVelocity = CalcDesiredVelocity();

        if (useRootMotion && applyRootMotion)
            movement.Move(desiredVelocity, speed, !allowVerticalMovement);
        else
        {
            // Move with acceleration and friction

            var currentFriction = isGrounded ? groundFriction : airFriction;
            var currentBrakingFriction = useBrakingFriction ? brakingFriction : currentFriction;

            movement.Move(desiredVelocity, speed, acceleration, deceleration, currentFriction,
                currentBrakingFriction, !allowVerticalMovement);
        }

        // Jump logic

        Jump();
        MidAirJump();
        UpdateJumpTimer();

        // Update root motion state,
        // should animator root motion be enabled? (eg: is grounded)

        applyRootMotion = useRootMotion && movement.isGrounded;
    }



    protected override void Animate()
    {
        if (animator == null)
            return;

        float speed = Vector3.ProjectOnPlane(movement.velocity, Vector3.up).magnitude;

        animator.SetFloat("MoveSpeed", speed);
        animator.SetBool("Run", moveDirection.sqrMagnitude > 0f);
        animator.SetBool("Jump", !isGrounded);
    }

    protected virtual void UpdateRotation(bool rotateTowards = true)
    {
        if (rotationChanging)
        {
            elapsedChangingTime += Time.deltaTime;

            if (elapsedChangingTime <= changingTime)
            {
                movement.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedChangingTime / changingTime);
            }
            else
            {
                elapsedChangingTime = 0.0f;

                rotationChanging = false;
            }
        }
        else if (rotateTowards)
        {
            if (useRootMotion && applyRootMotion && useRootMotionRotation)
            {
                // Use animation rotation to rotate our character

                movement.rotation *= animator.deltaRotation;
            }
            else
            {
                // Rotate towards movement direction (input)

                RotateTowardsMoveDirection();
            }
        }
    }

    private void ChangeRotation(Quaternion inTargetRotation, float inChangingTime)
    {
        startRotation = trans.rotation;
        targetRotation = inTargetRotation;

        elapsedChangingTime = 0.0f;
        changingTime = inChangingTime;
        rotationChanging = true;
    }

    private void BlockControl(float time)
    {
        controlBlock = true;
        elapsedControlBlockTime = 0f;
        controlBlockTime = time;


        moveDirectionRaw = Vector3.zero;
        moveDirection = moveDirectionRaw;

        mainAbilityAction1.Put(false);
        mainAbilityAction2.Put(false);

        jump = false;
        key_interact = false;
        key_f = false;
        key_alpha1 = false;
    }

    private void UpdateControlBlock()
    {
        if (!controlBlock)
            return;

        elapsedControlBlockTime += Time.deltaTime;

        if (elapsedControlBlockTime >= controlBlockTime)
        {
            controlBlock = false;
        }
    }


    /// <summary>
    /// �ǰ��̻��� �ߵ��մϴ�. �ǰ��̻��� Ȱ��ȭ�� ������ ĳ���Ϳ� �̵������� ������ �ʽ��ϴ�.
    /// </summary>
    /// <param name="hitDirection">�ǰ� ����</param>
    public void ActivateHitDisorder(Vector3 hitDirection)
    {
        hitDirection = Vector3.ProjectOnPlane(hitDirection, transform.up).normalized;
        hitDirection.y = 1.0f;

        movement.velocity = hitDirection * 10.0f;
        movement.DisableGrounding();

        elapsedHitDisorderTime = 0.0f;
        if (!hitDisordering)
        {
            hitDisordering = true;
            StartCoroutine(UpdateHitDisorder());
        }
    }

    private IEnumerator UpdateHitDisorder()
    {
        while (hitDisordering)
        {
            elapsedHitDisorderTime += Time.deltaTime;

            if (elapsedHitDisorderTime >= hitDisorderTime)
            {
                hitDisordering = false;
                break;
            }

            yield return null;
        }
    }

    private void RegistEvents()
    {
        if (animEventListener == null)
        {
            return;
        }

        animEventListener.OnEventEmitted[0] = HoldObject;
        animEventListener.OnEventEmitted[1] = PutObject;
    }

    private RaycastHit Raycast(float distance)
    {
        RaycastHit hit;
        Vector3 pos = trans.position + (Vector3.up * 0.7f);

        if (Physics.Raycast(pos, trans.TransformDirection(Vector3.forward), out hit, RAY_DISTANCE))
        {
#if UNITY_EDITOR
            Debug.DrawLine(pos, pos + (trans.TransformDirection(Vector3.forward) * hit.distance), Color.red);
#endif
        }

        return hit;
    }

    private bool RaycastForward(out RaycastHit hitInfo, float maxDistance, LayerMask layerMask)
    {
        Vector3 pos = transform.position + (transform.up * (col.height * 0.5f));

        return Physics.Raycast(pos, transform.forward, out hitInfo, maxDistance, layerMask);
    }


    public void Noclip()
    {
        noclipEnable = !noclipEnable;

        if (noclipEnable)
        {
            movement.DisableGroundDetection();
            CachedRigidbody.velocity = Vector3.zero;
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
            movement.EnableGroundDetection();
        }
    }

    private void NoclipMove()
    {

        float noclipSpeed = 45.0f;

        Vector3 desiredVelocity = moveDirection * noclipSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            desiredVelocity.y = 10f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            desiredVelocity.y = -10f;
        }


        CachedRigidbody.velocity = desiredVelocity;

    }


    // �ӽ� ���� Idle ����
    private IEnumerator RandomIdle()
    {
        while (true)
        {
            float randomTime = Random.Range(10.0f, 40.0f);
            yield return new WaitForSeconds(randomTime);
            animator.SetTrigger("RandomMotion");
        }
    }
}