using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    [Header("Climb")]
    [SerializeField] private float climbSpeed = 5.0f;


    #region <�ൿ �߰��� ����Ʈ �۾�>
    private void Climb_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Climb,
            new SimpleBehaviour(Climb_Enter, Climb_Update, Climb_FixedUpdate, Climb_Exit)
            );
    }

    private void Climb_Enter(PlayerState prev)
    {

    }

    private void Climb_Update()
    {
        
    }

    private void Climb_FixedUpdate()
    {

    }

    private void Climb_Exit(PlayerState next)
    {

    }
    #endregion


    private void Climb_GetInput()
    {

    }


}
