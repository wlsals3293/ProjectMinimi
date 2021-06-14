using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;


public partial class PlayerController : BaseCharacterController
{

    #region <�ൿ �߰��� ����Ʈ �۾�>
    private void Dead_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Dead
            , new SimpleBehaviour(Dead_Enter, Dead_Update, Dead_FixedUpdate, Dead_Exit));
    }

    private void Dead_Enter(PlayerState prev)
    {
        PlayerCharacter.SetHP(0);
    }

    private void Dead_Update()
    {
    }

    private void Dead_FixedUpdate()
    {
    }

    private void Dead_Exit(PlayerState next)
    {
        
    }
    #endregion
}
