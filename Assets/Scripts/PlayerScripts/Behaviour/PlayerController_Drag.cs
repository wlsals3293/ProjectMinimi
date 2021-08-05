using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using ECM.Common;

public partial class PlayerController : BaseCharacterController
{
    public float DragSpeed = 3.0f;

    Transform wagon = null;

    Vector3 DragDir = Vector3.zero;



    #region <�ൿ �߰��� ����Ʈ �۾�>
    private void Drag_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Drag
            , new SimpleBehaviour(Drag_Enter, Drag_Update, Drag_FixedUpdate, Drag_Exit));
    }

    private void Drag_Enter(PlayerState prev)
    {
        if(wagon != null)
        {
            Vector3 handle = wagon.Find("Handle").transform.position;
            transform.position = new Vector3(handle.x, transform.position.y, handle.z);
            DragDir = wagon.transform.forward;
            wagon.parent = this.transform;
            
            
        }
    }

    private void Drag_Update()
    {
        UpdateRotationChanging();
        Drag_GetInput();
    }

    private void Drag_FixedUpdate()
    {
        float MoveSpd = moveDirectionRaw.z * DragSpeed;

        Vector3 desiredVelocity =
            (DragDir * MoveSpd);

        movement.Move(desiredVelocity, DragSpeed, true);
    }

    private void Drag_Exit(PlayerState next)
    {
        if (wagon != null)
        {
            wagon.transform.parent = null;

            wagon = null;
            
        }

    }
    #endregion
    private void Drag_GetInput()
    {
        if (key_interact)
        {
            ChangeState(PlayerState.Idle);
        }
    }
}
