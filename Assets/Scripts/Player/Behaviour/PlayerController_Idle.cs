using ECM.Common;
using ECM.Controllers;
using UnityEngine;

public partial class PlayerController : BaseCharacterController
{

    #region <�ൿ �߰��� ����Ʈ �۾�>
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

        playerAbility.MainAction1(mainAbilityAction1);  // ��Ŭ��
        playerAbility.MainAction2(mainAbilityAction2);  // ��Ŭ��

        playerAbility.NumAction1(numAbilityAction1);    // Ű���� ����Ű 1
        playerAbility.NumAction2(numAbilityAction2);    // Ű���� ����Ű 2
        playerAbility.NumAction3(numAbilityAction3);    // Ű���� ����Ű 3


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
