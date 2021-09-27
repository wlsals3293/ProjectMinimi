using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using ECM.Common;

public partial class PlayerController : BaseCharacterController
{
    [Header("Aim")]

    [Tooltip("카메라가 따라가는 목표")]
    public Transform followTarget;

    [Tooltip("발사 위치")]
    public Transform firePoint;


    #region <행동 추가시 디폴트 작업>
    private void Aim_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Aim
            , new SimpleBehaviour(Aim_Enter, Aim_Update, Aim_FixedUpdate, Aim_Exit));
    }

    private void Aim_Enter(PlayerState prev)
    {
        Vector3 currentRot = movement.rotation.eulerAngles;
        currentRot.y = CameraT.rotation.eulerAngles.y;
        movement.rotation = Quaternion.Euler(currentRot);

        UIManager.Instance.HUD.VisibleCrossHair(true);
        CameraManager.Instance.ActivateCustomCamera(1);

        onRotationAxisInput += Aim_Rotate;
    }

    private void Aim_Update()
    {
        Aim_GetInput();
        Animate();

        if (playerAbility != null)
            playerAbility.AbilityUpdate();
    }

    private void Aim_FixedUpdate()
    {
        Move();
    }

    private void Aim_Exit(PlayerState next)
    {
        UIManager.Instance.HUD.VisibleCrossHair(false);
        CameraManager.Instance.ActivateCustomCamera(0);
    }
    #endregion


    private void Aim_GetInput()
    {
        playerAbility.MainAction1(mainAbilityAction1);
        playerAbility.MainAction2(mainAbilityAction2);

        playerAbility.NumAction1(numAbilityAction1);
        playerAbility.NumAction2(numAbilityAction2);
        playerAbility.NumAction3(numAbilityAction3);

        moveDirection = moveDirection.relativeTo(CameraT);
    }

    private void Aim_Rotate(float deltaX, float deltaY)
    {
        movement.rotation *= Quaternion.Euler(0.0f, deltaX, 0.0f);
    }
}
