using System;
using System.Collections;
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

    [Tooltip("피격이상 지속시간")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float hitDisorderTime = 0.6f;


    private Vector3 moveDirectionRaw;

    private Vector2 rotationInput;

    /// <summary>
    /// 플레이어가 이 높이보다 낮아지면 사망함
    /// </summary>
    private float killY;

    [Tooltip("마우스 수평 감도")]
    public float mouseHorizontalSensitivity = 3;

    [Tooltip("마우스 수직 감도")]
    public float mouseVerticalSensitivity = 3;


    [SerializeField]
    private AnimationEventListener animEventListener = null;



    private KeyInfo mainAbilityAction1;
    private KeyInfo mainAbilityAction2;


    private KeyInfo numAbilityAction1, numAbilityAction2, numAbilityAction3;

    private bool key_interact;  // E
    private bool key_f;         // F


    // 부드러운 캐릭터 회전
    private bool rotationChanging;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float elapsedChangingTime;
    private float changingTime;

    // 피격이상
    private bool hitDisordering;
    private TimerInstance hitDisorderingTimer;

    // 캐릭터 제어 일시 제한
    private bool controlStop = false;
    private TimerInstance controlStopTimer;



    private bool noclipEnable = false;



    private AnimationMovement animMovement;


    private CapsuleCollider col;

    private PlayerAbility playerAbility = null;

    private Transform hold_target = null;




    public delegate void TwoAxisDelegate(float deltaX, float deltaY);

    public TwoAxisDelegate onRotationAxisInput;


    public Rigidbody CachedRigidbody
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

        // TODO 중력 생각 2가지 pause
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ToggleESCMenu();
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            DebugController.Instance.OnToggleDebug();
        }


        // 마우스 움직임 입력
        rotationInput = new Vector2
        {
            x = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity,
            y = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity
        };

        onRotationAxisInput?.Invoke(rotationInput.x, rotationInput.y);


        // 조작 불가능한 상태면 리턴
        if (controlStop)
            return;

        // Handle user input
        moveDirectionRaw = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0.0f,
            z = Input.GetAxisRaw("Vertical")
        };
        moveDirection = moveDirectionRaw;

        // 주기술 (마우스 왼클릭, 오른클릭)
        mainAbilityAction1.Put(Input.GetMouseButton(0));
        mainAbilityAction2.Put(Input.GetMouseButton(1));

        // 키보드 숫자키
        numAbilityAction1.Put(Input.GetKeyDown(KeyCode.Alpha1));
        numAbilityAction2.Put(Input.GetKeyDown(KeyCode.Alpha2));
        numAbilityAction3.Put(Input.GetKeyDown(KeyCode.Alpha3));


        jump = Input.GetButton("Jump");
        key_interact = Input.GetKeyDown(KeyCode.E);
        key_f = Input.GetKeyDown(KeyCode.F);
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

    /// <summary>
    /// 플레이어의 조작을 일정 시간동안 막습니다.
    /// </summary>
    /// <param name="time">막을 시간</param>
    private void StopControl(float time)
    {
        controlStop = true;

        if (controlStopTimer == null)
        {
            controlStopTimer = Timer.SetTimer(this, () =>
            {
                controlStop = false;
                controlStopTimer = null;
            }, time);
        }
        else
        {
            if (controlStopTimer.TargetTime < time)
                controlStopTimer.Restart(time);
            else
                controlStopTimer.Restart();
        }

        moveDirectionRaw = Vector3.zero;
        moveDirection = moveDirectionRaw;

        mainAbilityAction1.Put(false);
        mainAbilityAction2.Put(false);

        numAbilityAction1.Put(false);
        numAbilityAction2.Put(false);
        numAbilityAction3.Put(false);

        jump = false;
        key_interact = false;
        key_f = false;
    }


    /// <summary>
    /// 피격이상을 발동합니다. 피격이상이 활성화된 동안은 캐릭터에 이동조작이 먹히지 않습니다.
    /// </summary>
    /// <param name="hitDirection">피격 방향</param>
    public void ActivateHitDisorder(Vector3 hitDirection)
    {
        hitDirection = Vector3.ProjectOnPlane(hitDirection, transform.up).normalized;
        hitDirection.y = 1.0f;

        movement.velocity = hitDirection * 10.0f;
        movement.DisableGrounding();

        if (!hitDisordering)
        {
            hitDisordering = true;
            hitDisorderingTimer = Timer.SetTimer(this, () =>
            {
                hitDisordering = false;
                hitDisorderingTimer = null;
            }, hitDisorderTime);
        }
        else
        {
            hitDisorderingTimer.Restart();
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

    /// <summary>
    /// 캐릭터의 중앙으로부터 캐릭터의 전방으로 레이캐스트를 합니다
    /// </summary>
    /// <param name="hitInfo">레이히트정보</param>
    /// <param name="maxDistance">최대거리</param>
    /// <param name="layerMask">레이어 마스크</param>
    /// <returns>레이캐스트 성공여부</returns>
    private bool RaycastForward(out RaycastHit hitInfo, float maxDistance, LayerMask layerMask)
    {
        Vector3 pos = transform.position + (transform.up * (col.height * 0.5f));

        bool result = Physics.Raycast(pos, transform.forward, out hitInfo, maxDistance,
            layerMask, QueryTriggerInteraction.Ignore);

#if UNITY_EDITOR
        if (result)
        {
            Debug.DrawLine(pos, pos + (transform.forward * hitInfo.distance), Color.red);
        }
#endif
        return result;
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
        float noclipSpeed = speed * 6f;

        Vector3 desiredVelocity = moveDirection * noclipSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            desiredVelocity.y += 10f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            desiredVelocity.y -= 10f;
        }


        CachedRigidbody.velocity = desiredVelocity;

    }


    // 임시 랜덤 Idle 구현
    private IEnumerator RandomIdle()
    {
        while (true)
        {
            float randomTime = UnityEngine.Random.Range(10.0f, 40.0f);
            yield return new WaitForSeconds(randomTime);
            animator.SetTrigger("RandomMotion");
        }
    }
}