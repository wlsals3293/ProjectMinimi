using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    [Header("Climb")]
    [SerializeField] private float climbingSpeed = 5.0f;

    private Vector3 climbFaceNormal = Vector3.zero;
    private Vector3 climbHorizontalForward = Vector3.zero;


    #region <�ൿ �߰��� ����Ʈ �۾�>
    private void Climb_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Climb,
            new SimpleBehaviour(Climb_Enter, Climb_Update, Climb_FixedUpdate, Climb_Exit)
            );
    }

    private void Climb_Enter(PlayerState prev)
    {
        Vector3 origin = trans.position + Vector3.up * 0.1f;

        if(Physics.Raycast(origin, -climbFaceNormal, out RaycastHit hit, RAY_DISTANCE))
        {
            Vector3 climbPos = hit.point + climbFaceNormal * 0.5f;
            Quaternion climbRot = Quaternion.LookRotation(-climbFaceNormal);

            movement.cachedRigidbody.MovePosition(climbPos);
            movement.cachedRigidbody.MoveRotation(climbRot);

            climbHorizontalForward = Vector3.ProjectOnPlane(-climbFaceNormal, Vector3.up).normalized;

            allowVerticalMovement = true;
            movement.capsuleCollider.isTrigger = true;
            movement.DisableGroundDetection();
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

    }

    private void Climb_Update()
    {
        Climb_GetInput();
        Climb_GrabEdge();
    }

    private void Climb_FixedUpdate()
    {
        Vector3 desiredVelocity = moveDirection.z * climbingSpeed * trans.up;

        movement.Move(desiredVelocity, climbingSpeed, !allowVerticalMovement);
    }

    private void Climb_Exit(PlayerState next)
    {
        movement.cachedRigidbody.MoveRotation(Quaternion.LookRotation(climbHorizontalForward));

        allowVerticalMovement = false;
        movement.capsuleCollider.isTrigger = false;
        movement.EnableGroundDetection();
    }
    #endregion


    private void Climb_GetInput()
    {
        if(key_interact)
        {
            ChangeState(PlayerState.Idle);
        }

        if(jump)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    /// <summary>
    /// �𼭸����� �ö�Դ��� �Ǵ��ϰ� �ö������ ĳ���͸� �Ű��ְ� Idle ���·� ���ư�
    /// TODO �� ����� ���� or ��Ʈ��� �ִϸ��̼� �̵� or �𼭸� ��� �پ��ֱ�
    /// </summary>
    private void Climb_GrabEdge()
    {
        Vector3 origin = trans.position + trans.up * 0.5f;

        if (!Physics.Raycast(origin, climbHorizontalForward, out RaycastHit hit, 1.0f))
        {
            origin = trans.position + Vector3.up * 1.0f + climbHorizontalForward * 0.6f;

            if(Physics.Raycast(origin, Vector3.down, out hit, 2.0f))
            {
                movement.cachedRigidbody.MovePosition(hit.point);
            }

            ChangeState(PlayerState.Idle);
        }
    }


}
