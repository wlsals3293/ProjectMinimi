using System.Collections;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    [Header("Behaviour Hold")]
    [SerializeField] private Transform pivotObjHolding = null;

    private Transform hold_target = null;

    #region <행동 추가시 디폴트 작업>
    private void Hold_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Hold
            , new SimpleBehaviour(Hold_Enter, Hold_Update, Hold_FixedUpdate, Hold_Exit));
    }

    private void Hold_Enter(PlayerState prev)
    {
        if (hold_target != null)
        {
            hold_target.parent = pivotObjHolding;
            hold_target.localPosition = Vector3.zero;
            Rigidbody rig = hold_target.GetComponent<Rigidbody>();
            if (rig != null)
                rig.isKinematic = true;
        }
    }

    private void Hold_Update()
    {
        Idle_Update();
    }

    private void Hold_FixedUpdate()
    {
        Move();
    }

    private void Hold_Exit(PlayerState next)
    {
        if (hold_target != null)
        {
            hold_target.parent = null;
            // TODO : 좌표지정 수정필요
            Vector3 pos = hold_target.localPosition;
            //pos.y = 1f;
            hold_target.localPosition = pos;
            Rigidbody rig = hold_target.GetComponent<Rigidbody>();
            if(rig != null)
                rig.isKinematic = false;

            hold_target = null;
        }
    }
    #endregion

    
}