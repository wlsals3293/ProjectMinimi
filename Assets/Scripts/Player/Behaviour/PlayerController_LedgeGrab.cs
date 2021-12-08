using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    [Header("Ledge Grab")]

    [Tooltip("기어올라가기 애니메이션 커브")]
    [SerializeField]
    private AnimCurve3 pullUpCurve;


    private float ledgeDistance;
    private float ledgeHeight;

    private Vector3 ledgeOriginColliderCenter;
    private float ledgeOriginColliderHeight;
    private float ledgeOriginColliderRadius;

    /// <summary>
    /// 잡고있는 물체(플랫폼같은 움직이는 물체)의 리지드바디
    /// </summary>
    private Rigidbody grabbedRigidbody;

    private Vector3 grabbedOffset;
    private Vector3 grabbedPosition;



    #region <행동 추가시 디폴트 작업>
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
    /// 모서리를 감지합니다.
    /// </summary>
    private void DetectLedge()
    {
        if (!isFalling)
            return;

        // 캐릭터 전방으로 레이를 쏴서 히트했는지 체크
        if (RaycastForward(out RaycastHit hit, ledgeDistance, LayerMasks.GO))
        {
            Vector3 grabForward = Vector3.ProjectOnPlane(-hit.normal, Vector3.up).normalized;
            Vector3 pos = hit.point + (grabForward * 0.2f) + (transform.up * ledgeHeight);


            // 첫번째 레이가 히트한 위치를 기준으로 위에서 아래로 다시 레이를 쏴 히트했는지 체크
            if (Physics.Raycast(pos, -transform.up, out RaycastHit hit2, 0.2f,
                LayerMasks.GO, QueryTriggerInteraction.Ignore))
            {
                float dot = Vector3.Dot(hit2.normal, Vector3.up);

                // 히트한 땅의 각도가 대략 30도보다 크면 리턴
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


                // 매달리고 있을 위치로 조정
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
    /// 다른 물체나 땅에 밀려서 원래 잡은 위치로부터 일정거리 이상 떨어지면 Idle로 돌아갑니다.
    /// </summary>
    /// <returns>떨어지면 true 반환</returns>
    private bool CheckDetach()
    {
        if (grabbedRigidbody != null)
        {
            float distanceSqr =
            (grabbedRigidbody.transform.TransformPoint(grabbedOffset) - CachedRigidbody.position)
            .sqrMagnitude;

            // 잡았던 지점으로부터 0.4 보다 멀어지면 
            if (distanceSqr > 0.16f)
            {
                ChangeState(PlayerState.Idle);
                return true;
            }
        }
        else
        {
            float distanceSqr = (grabbedPosition - CachedRigidbody.position).sqrMagnitude;

            // 잡았던 지점으로부터 0.4 보다 멀어지면 
            if (distanceSqr > 0.16f)
            {
                ChangeState(PlayerState.Idle);
                return true;
            }
        }

        return false;
    }

}
