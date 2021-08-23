using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public partial class PlayerController : BaseCharacterController
{
    private float ledgeDistance;
    private float ledgeHeight;

    private Vector3 ledgeUpMovePosition;

    private Rigidbody grabbedRigidbody;



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
        movement.velocity = Vector3.zero;
        movement.capsuleCollider.isTrigger = true;
        movement.DisableGroundDetection();

        animator.SetBool("LedgeGrab", true);
    }

    private void LedgeGrab_Update()
    {
        LedgeGrab_GetInput();
    }

    private void LedgeGrab_FixedUpdate()
    {
        ApplyPlatformMovement();
    }

    private void LedgeGrab_Exit(PlayerState next)
    {
        grabbedRigidbody = null;
        movement.capsuleCollider.isTrigger = false;
        movement.EnableGroundDetection();

        animator.SetBool("LedgeGrab", false);
    }
    #endregion

    private void LedgeGrab_GetInput()
    {
        //TODO 나중에 애니메이션 추가 되면 수정
        /*if (jump && moveDirection.z > 0f)
        {
            movement.cachedRigidbody.MovePosition(ledgeUpMovePosition);
            ChangeState(PlayerState.Idle);
        }*/
        if (jump && _canJump)
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
        if (RaycastForward(out RaycastHit hit, ledgeDistance, LayerMasks.Ground))
        {
            Vector3 grabForward = Vector3.ProjectOnPlane(-hit.normal, Vector3.up).normalized;
            Vector3 pos = hit.point + (grabForward * 0.4f) + (transform.up * ledgeHeight);


            // 첫번째 레이가 히트한 위치를 기준으로 위에서 아래로 다시 레이를 쏴 히트했는지 체크
            if (Physics.Raycast(pos, -transform.up, out RaycastHit hit2, 0.3f, LayerMasks.Ground))
            {
                // 히트한 땅의 각도가 대략 45도보다 크면 리턴
                float dot = Vector3.Dot(hit2.normal, Vector3.up);

                if (dot < 0.7f)
                    return;


                ledgeUpMovePosition = hit2.point;   // 올라가는 애니메이션 후 있을 위치 (임시)

                grabbedRigidbody = hit2.rigidbody;


                // 매달리고 있을 위치로 조정
                Vector3 position = new Vector3(hit.point.x, hit2.point.y - 1.1f, hit.point.z);
                position += -grabForward * (col.radius + 0.01f);
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
}
