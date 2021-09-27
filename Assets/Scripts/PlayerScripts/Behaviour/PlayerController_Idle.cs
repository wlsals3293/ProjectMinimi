using ECM.Common;
using ECM.Controllers;
using UnityEngine;

public partial class PlayerController : BaseCharacterController
{
    private InteractType interactType = InteractType.None;


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

    }

    private void Idle_FixedUpdate()
    {
        Move();
        DetectLedge();
    }

    private void Idle_Exit(PlayerState next)
    {

    }
    #endregion


    private void Idle_GetInput()
    {
        RaycastHit hit = Raycast(RAY_DISTANCE);
        interactType = UpdateInteractActionType(hit);

        if (key_interact)
        {
            Interact_Action(hit, interactType);
        }

        playerAbility.MainAction1(mainAbilityAction1);  // 좌클릭
        playerAbility.MainAction2(mainAbilityAction2);  // 우클릭

        playerAbility.NumAction1(numAbilityAction1);    // 키보드 숫자키 1
        playerAbility.NumAction2(numAbilityAction2);    // 키보드 숫자키 2
        playerAbility.NumAction3(numAbilityAction3);    // 키보드 숫자키 3


        moveDirection = moveDirection.relativeTo(CameraT, !noclipEnable);
    }

    public void Interact_Action(RaycastHit hit, InteractType keyType)
    {
        switch (keyType)
        {
            case InteractType.None:
                break;
            case InteractType.Block:
                climbFaceNormal = hit.normal;
                fsm.ChangeState(PlayerState.Climb);
                break;
            case InteractType.Hold:
                fsm.ChangeState(PlayerState.Hold);
                break;
            case InteractType.Wagon:
                fsm.ChangeState(PlayerState.Drag);
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
            if (hit.collider.CompareTag(Tags.Object))
            {
                hold_target = hit.transform;

                return InteractType.Hold;
            }

            else if (hit.collider.CompareTag(Tags.Wagon))
            {
                dragObject = hit.transform;

                return InteractType.Wagon;
            }
        }

        return InteractType.None;
    }

    public void DrawLineRaycatAllways(float distance = 0f)
    {
        if (distance != 0)
        {
            Raycast(distance);
        }
        else
        {
            Raycast(RAY_DISTANCE);
        }
    }

}
