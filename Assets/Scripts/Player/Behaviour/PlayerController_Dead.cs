using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;


public partial class PlayerController : BaseCharacterController
{

    #region <행동 추가시 디폴트 작업>
    private void Dead_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Dead
            , new SimpleBehaviour(Dead_Enter, Dead_Update, Dead_FixedUpdate, Dead_Exit));
    }

    private void Dead_Enter(PlayerState prev)
    {
        pause = true;
        PlayerCharacter.SetHP(0);

        UIManager.Instance.OpenView(UIManager.EUIView.Death);
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
