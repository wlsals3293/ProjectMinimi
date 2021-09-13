using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using ECM.Common;

public partial class PlayerController : BaseCharacterController
{
    public float dragSpeed = 3.0f;

    Transform wagon = null;

    Vector3 dragDir = Vector3.zero;



    #region <행동 추가시 디폴트 작업>
    private void Drag_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Drag
            , new SimpleBehaviour(Drag_Enter, Drag_Update, Drag_FixedUpdate, Drag_Exit));
    }

    private void Drag_Enter(PlayerState prev)
    {
        if (wagon != null)
        {
            transform.rotation = wagon.rotation;
            Vector3 handle = wagon.Find("PlayerLocation").transform.position;
            transform.position = new Vector3(handle.x, transform.position.y, handle.z);
            dragDir = wagon.transform.forward;
            wagon.parent = this.transform;

            animator.SetTrigger("ToDrag");
            animator.SetBool("Drag", true);
        }
        else
            ChangeState(PlayerState.Idle);
    }

    private void Drag_Update()
    {
        Drag_GetInput();
    }

    private void Drag_FixedUpdate()
    {
        float MoveSpd = moveDirectionRaw.z * dragSpeed;

        Vector3 desiredVelocity =
            (dragDir * MoveSpd);

        movement.Move(desiredVelocity, dragSpeed, true);
    }

    private void Drag_Exit(PlayerState next)
    {
        if (wagon != null)
        {
            wagon.transform.parent = null;

            wagon = null;

        }
        animator.SetBool("Drag", false);
        animator.SetBool("Push", false);
        animator.SetBool("Pull", false);
    }
    #endregion
    private void Drag_GetInput()
    {
        if (key_interact)
        {
            ChangeState(PlayerState.Idle);
        }

        if (moveDirectionRaw.z > 0f)
            animator.SetBool("Push", true);
        else if (moveDirectionRaw.z < 0f)
            animator.SetBool("Pull", true);
        else
        {
            animator.SetBool("Push", false);
            animator.SetBool("Pull", false);
        }
    }
}
