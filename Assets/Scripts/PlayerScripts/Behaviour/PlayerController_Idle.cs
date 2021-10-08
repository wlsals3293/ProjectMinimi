using ECM.Common;
using ECM.Controllers;
using UnityEngine;

public partial class PlayerController : BaseCharacterController
{

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
        if (key_interact)
        {
            Interact();
        }

        playerAbility.MainAction1(mainAbilityAction1);  // 좌클릭
        playerAbility.MainAction2(mainAbilityAction2);  // 우클릭

        playerAbility.NumAction1(numAbilityAction1);    // 키보드 숫자키 1
        playerAbility.NumAction2(numAbilityAction2);    // 키보드 숫자키 2
        playerAbility.NumAction3(numAbilityAction3);    // 키보드 숫자키 3


        moveDirection = moveDirection.relativeTo(CameraT, !noclipEnable);
    }

    public void Interact()
    {
        if (RaycastForward(out RaycastHit hit, RAY_DISTANCE, LayerMasks.Object))
        {
            hit.collider.GetComponent<IInteractable>()?.Interact(this);
        }
    }

}
