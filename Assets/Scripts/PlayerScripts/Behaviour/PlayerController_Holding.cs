using System.Collections;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("Behaviour Holding")]
    [SerializeField] private Transform pivotObjHolding = null;

    private Transform hold_target = null;

    #region <행동 추가시 디폴트 작업>
    private void Holding_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Holding
            , new SimpleBehaviour(Holding_Enter, Holding_Update, Holding_Exit));
    }

    private void Holding_Enter(PlayerState prev)
    {
        if (hold_target != null)
        {
            hold_target.parent = pivotObjHolding;
            hold_target.localPosition = Vector3.zero;
        }
    }

    private void Holding_Update()
    {
        Idle_Update();
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

            hold_target = null;
        }
    }
    #endregion

    
}