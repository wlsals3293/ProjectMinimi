using ECM.Common;
using ECM.Controllers;
using UnityEngine;

public partial class PlayerController : BaseCharacterController
{
    private InteractType interactType = InteractType.None;

    private delegate void IdleUpdateDelegate();
    private IdleUpdateDelegate onIdleUpdate;



    #region <행동 추가시 디폴트 작업>
    private void Idle_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Idle
            , new SimpleBehaviour(Idle_Enter, Idle_Update, Idle_FixedUpdate, Idle_Exit));
    }

    private void Idle_Enter(PlayerState prev)
    {

    }

    private void Idle_Update()
    {
        Idle_GetInput();
        UpdateRotation();
        UpdateControlBlock();
        Animate();

        if (playerAbility != null)
            playerAbility.AbilityUpdate();

        onIdleUpdate?.Invoke();
    }

    private void Idle_FixedUpdate()
    {
        Move();
        DetectLedge();
    }
    
    private void Idle_Exit(PlayerState next)
    {
        MinimiManager.Instance.UnDrawBlueprintObject();

        onIdleUpdate -= MinimiManager.Instance.DrawBlueprintObject;
    }
    #endregion


    private void Idle_GetInput()
    {
        // TODO 준비단계가 필요없으면 input e안으로
        RaycastHit hit = Raycast(RAY_DISTANCE);
        interactType = UpdateInteractActionType(hit);

        if (key_interact)
        {
            Debug.Log("Input E Key");
            Interact_Action(hit, interactType);
        }


        // 좌클릭
        /*if (abilityAction1.down)
        {
            if (MinimiManager.Instance.InstallMinimi())
            {
                onIdleUpdate -= MinimiManager.Instance.DrawBlueprintObject;
            }
        }*/
        playerAbility.MainAction1(mainAbilityAction1);

        // 우클릭
        /*if (mainAbilityAction2.down)
        {
            MinimiManager.Instance.PutInAllMinimis();
            onIdleUpdate -= MinimiManager.Instance.DrawBlueprintObject;
        }*/

        playerAbility.MainAction2(mainAbilityAction2);

        // 블럭 미니미
        /*if (key_alpha1)
        {
            if(MinimiManager.Instance.TakeOutMinimi(MinimiType.Block))
            {
                onIdleUpdate += MinimiManager.Instance.DrawBlueprintObject;
            }
        }*/

        // F Key
        /*if (key_f)
        {
            MinimiManager.Instance.UninstallMinimi();
        }*/

        moveDirection = moveDirection.relativeTo(CameraT);
    }

    public void Interact_Action(RaycastHit hit, InteractType keyType)
    {
        switch (keyType)
        { 
            case InteractType.None:
                break;
            case InteractType.Block:
                Interact_Action_Block(hit);
                break;
            case InteractType.Hold:
                Interact_Action_Hold();
                break;
            case InteractType.Wagon:
                Interact_Action_Drag();
                break;
        
        }
    }

    private InteractType UpdateInteractActionType(RaycastHit hit)
    {
        int layer = -1;

        if (hit.collider != null)
        {
            layer = hit.collider.gameObject.layer;
        }

        if (layer == Layers.Minimi)
        {
            return InteractType.Block;
        }
        else if (layer == Layers.Obj)
        {
            if (fsm.CurState == PlayerState.Idle && hit.collider.CompareTag(Tags.Object))
            {
                hold_target = hit.transform;

                return InteractType.Hold;
            }

            else if (fsm.CurState == PlayerState.Idle && hit.collider.CompareTag(Tags.Wagon))
            {
                wagon = hit.transform;

                return InteractType.Wagon;
            }
        }

        return InteractType.None;
    }


    private void Interact_Action_Hold()
    {
        switch (fsm.CurState)
        {
            case PlayerState.Idle:
                fsm.ChangeState(PlayerState.Hold);
                break;
            case PlayerState.Hold:
                fsm.ChangeState(PlayerState.Idle);
                break;
        }
    }
    private void Interact_Action_Block(RaycastHit hit)
    {
        climbFaceNormal = hit.normal;
        ChangeState(PlayerState.Climb);
    }

    private void Interact_Action_Drag()
    {
        switch (fsm.CurState)
        {
            case PlayerState.Idle:
                fsm.ChangeState(PlayerState.Drag);
                break;
            case PlayerState.Drag:
                fsm.ChangeState(PlayerState.Idle);
                break;
        }
    }

    public void DrawLineRaycatAllways(float distance = 0f)
    {
        if(distance != 0)
        {
            Raycast(distance);
        }
        else
        {
            Raycast(RAY_DISTANCE);
        }
    }

}
