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
    [Header("Player Controller")]

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
    public float mouseHorizontalSensitivity = 4;

    [Tooltip("마우스 수직 감도")]
    public float mouseVerticalSensitivity = 4;



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

        playerCharacter = GetComponent<PlayerCharacter>();
        animator = GetComponentInChildren<Animator>();
        playerAbility = GetComponent<PlayerAbility>();

        Idle_SetState();
        Hold_SetState();
        Dead_SetState();
        Climb_SetState();
        Sliding_SetState();
        Aim_SetState();

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
        moveDirectionRaw = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0.0f,
            z = Input.GetAxisRaw("Vertical")
        };
        moveDirection = moveDirectionRaw;


        // 마우스 움직임 입력
        rotationInput = new Vector2
        {
            x = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity,
            y = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity
        };

        if(onRotationAxisInput != null)
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

        bool isRun = movement.velocity.sqrMagnitude > 9.0f;

        animator.SetBool("Run", isRun);
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