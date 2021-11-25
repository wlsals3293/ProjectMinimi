using ECM.Common;
using ECM.Controllers;
using UnityEngine;

public partial class PlayerController : BaseCharacterController
{
    [Header("Hold")]

    [SerializeField]
    private Transform holdingPivot = null;


    [SerializeField]
    private float holdMoveSpeed = 4f;

    private float savedMoveSpeed;



    #region <행동 추가시 디폴트 작업>
    private void Hold_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Hold
            , new SimpleBehaviour(Hold_Enter, Hold_Update, Hold_FixedUpdate, Hold_Exit));
    }

    private void Hold_Enter(PlayerState prev)
    {
        Vector3 lookDir = hold_target.position - transform.position;
        lookDir = Vector3.ProjectOnPlane(lookDir, Vector3.up).normalized;
        Quaternion qut = Quaternion.LookRotation(lookDir);

        ChangeRotation(qut, 0.2f);
        StopControl(0.55f);

        savedMoveSpeed = speed;
        speed = holdMoveSpeed;

        animator.SetTrigger("Pickup");
        animator.SetBool("Hold", true);
    }

    private void Hold_Update()
    {
        Hold_GetInput();
        UpdateRotation();
        Animate();
    }

    private void Hold_FixedUpdate()
    {
        Move();
    }

    private void Hold_Exit(PlayerState next)
    {
        StopControl(0.6f);
        speed = savedMoveSpeed;
        animator.SetBool("Hold", false);
    }
    #endregion


    private void Hold_GetInput()
    {
        if (key_interact)
            fsm.ChangeState(PlayerState.Idle);

        moveDirection = moveDirection.relativeTo(CameraT);
    }

    public void Hold(Transform target)
    {
        if (target != null)
        {
            hold_target = target;
            fsm.ChangeState(PlayerState.Hold);
        }
    }

    private void HoldObject()
    {
        if (hold_target == null)
            return;

        hold_target.parent = holdingPivot;
        hold_target.localPosition = Vector3.zero;

        Collider col = hold_target.GetComponent<Collider>();
        if (col != null)
        {
            col.attachedRigidbody.isKinematic = true;
            col.enabled = false;
        }
    }

    private void PutObject()
    {
        if (hold_target == null)
            return;

        hold_target.parent = null;

        Collider col = hold_target.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
            col.attachedRigidbody.isKinematic = false;

            Vector3 throwVec = transform.forward;
            throwVec.y = 3f;
            throwVec = throwVec * 2f;

            col.attachedRigidbody.velocity = throwVec;
        }

        hold_target = null;
    }
}