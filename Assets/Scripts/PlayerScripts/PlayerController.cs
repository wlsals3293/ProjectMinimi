using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;


public partial class PlayerController : BaseCharacterController
{

    
    private bool leftClick;
    private bool rightClick;

    private bool key_alpha1, key_alpha2, key_alpha3;    // 1, 2, 3
    private bool key_interact;  // E
    private bool key_f;         // F



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

        Idle_SetState();
        Holding_SetState();
    }

    private void Start()
    {
        MinimiManager._instance.playerTrans = trans;
    }

    public void Init()
    {
        ChangeState(PlayerState.Idle);
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



    private string GetHitTag()
    {
        return null;
    }

    public void SetLocalPosition(Vector3 pos)
    {
        transform.position = pos;
    }


}