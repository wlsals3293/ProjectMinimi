using ECM.Controllers;
using UnityEngine;

public partial class PlayerController : BaseCharacterController
{
    [Header("Drag")]


    [SerializeField]
    private float dragSpeed = 3.0f;

    private Vector3 dragDir = Vector3.zero;



    #region <행동 추가시 디폴트 작업>
    private void Drag_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Drag
            , new SimpleBehaviour(Drag_Enter, Drag_Update, Drag_FixedUpdate, Drag_Exit));
    }

    private void Drag_Enter(PlayerState prev)
    {
        movement.velocity = Vector3.zero;
        transform.rotation = hold_target.rotation;
        Vector3 handle = hold_target.Find("PlayerLocation").transform.position;
        transform.position = new Vector3(handle.x, transform.position.y, handle.z);
        dragDir = hold_target.transform.forward;
        hold_target.parent = transform;

        animator.SetTrigger("ToDrag");
        animator.SetBool("Drag", true);
    }

    private void Drag_Update()
    {
        Drag_GetInput();
        Drag_Animate();
    }

    private void Drag_FixedUpdate()
    {
        float MoveSpd = moveDirectionRaw.z * dragSpeed;

        Vector3 desiredVelocity =
            (dragDir * MoveSpd);

        movement.Move(desiredVelocity, dragSpeed, acceleration, deceleration,
            groundFriction, groundFriction, true);
    }

    private void Drag_Exit(PlayerState next)
    {
        if (hold_target != null)
        {
            hold_target.transform.parent = null;
            hold_target = null;
        }
        animator.SetBool("Drag", false);
        animator.SetBool("Push", false);
        animator.SetBool("Pull", false);
    }
    #endregion

    public void Drag(Transform target)
    {
        if (target != null)
        {
            hold_target = target;
            fsm.ChangeState(PlayerState.Drag);
        }
    }

    private void Drag_GetInput()
    {
        if (key_interact)
            fsm.ChangeState(PlayerState.Idle);
    }

    private void Drag_Animate()
    {
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
