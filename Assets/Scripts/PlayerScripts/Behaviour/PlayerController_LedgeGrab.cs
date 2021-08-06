using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{

    private bool ledgeEnable;



    #region <행동 추가시 디폴트 작업>
    private void LedgeGrab_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.LedgeGrab
            , new SimpleBehaviour(LedgeGrab_Enter, LedgeGrab_Update, LedgeGrab_FixedUpdate, LedgeGrab_Exit));
    }

    private void LedgeGrab_Enter(PlayerState prev)
    {
    }

    private void LedgeGrab_Update()
    {
    }

    private void LedgeGrab_FixedUpdate()
    {
    }

    private void LedgeGrab_Exit(PlayerState next)
    {
    }
    #endregion


    private void DetectLedge()
    {
        if(movement.cachedRigidbody.velocity.y < 0.0f)
        {
            Vector3 pos = transform.position + transform.TransformDirection(new Vector3(0.0f, 1.0f, 0.5f));
            if (Physics.Raycast(pos, transform.forward, out RaycastHit hit, 0.5f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
            {

            }
        }
    }
}
