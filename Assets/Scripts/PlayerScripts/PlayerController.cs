using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;


public partial class PlayerController : BaseCharacterController
{
    // 현재속도 확인용. 디버그 전용
    [SerializeField, ReadOnly]
    private float currentSpeed;

    /// <summary>
    /// 플레이어가 이 높이보다 낮아지면 사망함
    /// </summary>
    private float killY;


    private bool leftClick;
    private bool rightClick;

    private bool key_alpha1, key_alpha2, key_alpha3;    // 1, 2, 3
    private bool key_interact;  // E
    private bool key_f;         // F


    private bool rotationChanging;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float elapsedChangingTime;
    private float changingTime;

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


    private Transform cameraT
    {
        get
        {
            return CameraManager.Instance.GetMoveDirCamera();
        }
    }

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

    private FSMController fsm = new FSMController();


    private delegate void StateUpdateDelegate();



    public override void Awake()
    {
        base.Awake();

        playerCharacter = GetComponent<PlayerCharacter>();
        animator = GetComponentInChildren<Animator>();

        Idle_SetState();
        Hold_SetState();
        Dead_SetState();
        Climb_SetState();
        Sliding_SetState();
    }

    private void Start()
    {
        MinimiManager.Instance.playerTrans = transform;
    }

    public void Init()
    {
        killY = StageManager.Instance.globalKillY;
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

        if(fsm.CurState != PlayerState.None && fsm.CurState != PlayerState.Dead)
        {
            if(movement.cachedRigidbody.position.y < killY)
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

        if (Input.GetKeyDown(KeyCode.P))
            pause = !pause;

        // Handle user input
        moveDirection = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0.0f,
            z = Input.GetAxisRaw("Vertical")
        };

        leftClick = Input.GetMouseButtonDown(0);
        rightClick = Input.GetMouseButtonDown(1);

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



    private string GetHitTag()
    {
        return null;
    }

    public void SetLocalPosition(Vector3 pos)
    {
        transform.position = pos;
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
        if(!rotationChanging)
        {
            return;
        }

        elapsedChangingTime += Time.deltaTime;

        if(elapsedChangingTime <= changingTime)
        {
            movement.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedChangingTime / changingTime);
        }
        else
        {
            elapsedChangingTime = 0.0f;

            rotationChanging = false;
        }
    }
}