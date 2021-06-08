using System.Collections;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    [Header("Behaviour Holding")]
    [SerializeField] private Transform pivotObjHolding = null;

    private Transform hold_target = null;

    #region <행동 추가시 디폴트 작업>
    private void Holding_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Holding
            , new SimpleBehaviour(Holding_Enter, Holding_Update, Holding_FixedUpdate, Holding_Exit));
    }

    private void Holding_Enter(PlayerState prev)
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

    private void Holding_Update()
    {
        Idle_Update();
    }

    private void Holding_FixedUpdate()
    {

    }

    private void Holding_Exit(PlayerState next)
    {
        if (hold_target != null)
        {
            hold_target.parent = null;
            // TODO : 좌표지정 수정필요
            Vector3 pos = hold_target.localPosition;
            pos.y = 1f;
            hold_target.localPosition = pos;
            Rigidbody rig = hold_target.GetComponent<Rigidbody>();
            if(rig != null)
                rig.isKinematic = false;

            hold_target = null;
        }
    }
    #endregion

    
}