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

    [Tooltip("피격이상 지속시간")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float hitDisorderTime = 0.6f;


    // 현재속도 확인용. 디버그 전용
    [SerializeField, ReadOnly]
    private float currentSpeed;

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

        currentSpeed = movement.velocity.magnitude;
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

        // Handle user input
        if (!hitDisordering)
            moveDirectionRaw = new Vector3
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = 0.0f,
                z = Input.GetAxisRaw("Vertical")
            };
        else
            moveDirectionRaw = Vector3.zero;

        moveDirection = moveDirectionRaw;


        // 마우스 움직임 입력
        rotationInput = new Vector2
        {
            x = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity,
            y = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity
        };

        if (onRotationAxisInput != null)
            onRotationAxisInput(rotationInput.x, rotationInput.y);

        // 주기술 (마우스 왼클릭, 오른클릭)
        mainAbilityAction1.Put(Input.GetMouseButton(0));
        mainAbilityAction2.Put(Input.GetMouseButton(1));

        jump = Input.GetButton("Jump");
        key_interact = Input.GetKeyDown(KeyCode.E);
        key_f = Input.GetKeyDown(KeyCode.F);
        key_alpha1 = Input.GetKeyDown(KeyCode.Alpha1);

    }

    protected override void Animate()
    {
        if (animator == null)
            return;

        bool isRun = Vector3.ProjectOnPlane(movement.velocity, Vector3.up).sqrMagnitude > 9.0f;
            

        animator.SetBool("Run", isRun);
        animator.SetBool("Jump", !isGrounded);
    }

    private void ChangeRotation(Quaternion inTargetRotation, float inChangingTime)
    {
        startRotation = trans.rotation;
        targetRotation = inTargetRotation;

        elapsedChangingTime = 0.0f;
        changingTime = inChangingTime;
        rotationChanging = true;

    }

    private void UpdateRotationChanging()
    {
        if (!rotationChanging)
        {
            return;
        }

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

    // 임시 랜덤 Idle 구현
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