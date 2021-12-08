using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    [Header("Ledge Grab")]

    [Tooltip("���ö󰡱� �ִϸ��̼� Ŀ��")]
    [SerializeField]
    private AnimCurve3 pullUpCurve;


    private float ledgeDistance;
    private float ledgeHeight;

    private Vector3 ledgeOriginColliderCenter;
    private float ledgeOriginColliderHeight;
    private float ledgeOriginColliderRadius;

    /// <summary>
    /// ����ִ� ��ü(�÷������� �����̴� ��ü)�� ������ٵ�
    /// </summary>
    private Rigidbody grabbedRigidbody;

    private Vector3 grabbedOffset;
    private Vector3 grabbedPosition;



    #region <�ൿ �߰��� ����Ʈ �۾�>
    private void LedgeGrab_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.LedgeGrab
            , new SimpleBehaviour(LedgeGrab_Enter, LedgeGrab_Update, LedgeGrab_FixedUpdate, LedgeGrab_Exit));

        ledgeDistance = col.radius + 0.4f;
        ledgeHeight = col.height * 0.5f;
    }

    private void LedgeGrab_Enter(PlayerState prev)
    {
        allowVerticalMovement = true;
        movement.velocity = Vector3.zero;
        movement.DisableGroundDetection();

        ledgeOriginColliderCenter = col.center;
        ledgeOriginColliderHeight = col.height;
        ledgeOriginColliderRadius = col.radius;

        movement.SetCapsuleHeight(1.2f);

        animator.SetBool("LedgeGrab", true);
        animator.SetBool("LedgeGrabUp", false);
    }

    private void LedgeGrab_Update()
    {
        if (animMovement.IsActive)
        {
            if (animMovement.UpdatePosition())
            {
                ChangeState(PlayerState.Idle);
            }
        }
        else
        {
            LedgeGrab_GetInput();
        }
    }

    private void LedgeGrab_FixedUpdate()
    {
        ApplyPlatformMovement();

        if (!animMovement.IsActive)
        {
            CheckDetach();
        }
    }

    private void LedgeGrab_Exit(PlayerState next)
    {
        movement.SetCapsuleDimensions(
            ledgeOriginColliderCenter,
            ledgeOriginColliderRadius,
            ledgeOriginColliderHeight
            );
        grabbedRigidbody = null;
        allowVerticalMovement = false;
        movement.EnableGroundDetection();

        animator.SetBool("LedgeGrab", false);
        animator.SetBool("LedgeGrabUp", false);
    }
    #endregion

    private void LedgeGrab_GetInput()
    {
        if (moveDirection.z > 0f)
        {
            Vector3 endPos = transform.position + transform.forward * 0.5f + transform.up * 1.1f;

            movement.SetCapsuleDimensions(new Vector3(0f, 0.85f, 0f), 0.205f, 0.72f);

            animMovement.StartMovement(transform.position, endPos, 1.53f, pullUpCurve);
            animator.SetBool("LedgeGrabUp", true);
        }
        else if (jump && _canJump)
        {
            ChangeState(PlayerState.Idle);
            ExtraJump();
        }
    }

    /// <summary>
    /// �𼭸��� �����մϴ�.
    /// </summary>
    private void DetectLedge()
    {
        if (!isFalling)
            return;

        // ĳ���� �������� ���̸� ���� ��Ʈ�ߴ��� üũ
        if (RaycastForward(out RaycastHit hit, ledgeDistance, LayerMasks.GO))
        {
            Vector3 grabForward = Vector3.ProjectOnPlane(-hit.normal, Vector3.up).normalized;
            Vector3 pos = hit.point + (grabForward * 0.2f) + (transform.up * ledgeHeight);


            // ù��° ���̰� ��Ʈ�� ��ġ�� �������� ������ �Ʒ��� �ٽ� ���̸� �� ��Ʈ�ߴ��� üũ
            if (Physics.Raycast(pos, -transform.up, out RaycastHit hit2, 0.2f,
                LayerMasks.GO, QueryTriggerInteraction.Ignore))
            {
                float dot = Vector3.Dot(hit2.normal, Vector3.up);

                // ��Ʈ�� ���� ������ �뷫 30������ ũ�� ����
                if (dot < 0.866025f)
                    return;


                grabbedRigidbody = hit2.rigidbody;
                if (grabbedRigidbody != null)
                {
                    grabbedOffset =
                    grabbedRigidbody.transform.InverseTransformPoint(CachedRigidbody.position);
                }
                else
                {
                    grabbedPosition = CachedRigidbody.position;
                }


                // �Ŵ޸��� ���� ��ġ�� ����
                Vector3 position = new Vector3(hit.point.x, hit2.point.y - 1.1f, hit.point.z);
                position += -grabForward * 0.29f;
                Quaternion rotation = Quaternion.LookRotation(grabForward);

                transform.SetPositionAndRotation(position, rotation);


                ChangeState(PlayerState.LedgeGrab);
            }
        }
    }

    private void ApplyPlatformMovement()
    {
        if (grabbedRigidbody == null)
            return;

        CachedRigidbody.velocity = grabbedRigidbody.GetPointVelocity(CachedRigidbody.position);
    }

    /// <summary>
    /// �ٸ� ��ü�� ���� �з��� ���� ���� ��ġ�κ��� �����Ÿ� �̻� �������� Idle�� ���ư��ϴ�.
    /// </summary>
    /// <returns>�������� true ��ȯ</returns>
    private bool CheckDetach()
    {
        if (grabbedRigidbody != null)
        {
            float distanceSqr =
            (grabbedRigidbody.transform.TransformPoint(grabbedOffset) - CachedRigidbody.position)
            .sqrMagnitude;

            // ��Ҵ� �������κ��� 0.4 ���� �־����� 
            if (distanceSqr > 0.16f)
            {
                ChangeState(PlayerState.Idle);
                return true;
            }
        }
        else
        {
            float distanceSqr = (grabbedPosition - CachedRigidbody.position).sqrMagnitude;

            // ��Ҵ� �������κ��� 0.4 ���� �־����� 
            if (distanceSqr > 0.16f)
            {
                ChangeState(PlayerState.Idle);
                return true;
            }
        }

        return false;
    }

}
