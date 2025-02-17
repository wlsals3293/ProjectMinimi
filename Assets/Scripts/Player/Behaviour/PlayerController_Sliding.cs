using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using ECM.Common;

public partial class PlayerController : BaseCharacterController
{
    [Header("Sliding")]

    [Tooltip("슬라이딩 속도")]
    [SerializeField]
    private float slidingSpeed = 20.0f;

    [Tooltip("좌우 슬라이딩 속도")]
    [SerializeField]
    private float slidingSideSpeed = 15.0f;

    /// <summary>
    /// 다른 상태에서 슬라이딩으로 전환되는 시간
    /// </summary>
    private float slidingEnterTime = 0.2f;

    private float crashSpeedFactor = 1.0f;

    private SlidingSlope slidingSlope = null;

    private Vector3 slidingForward = Vector3.zero;
    private Vector3 slidingRight = Vector3.zero;


    #region <행동 추가시 디폴트 작업>
    private void Sliding_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Sliding
            , new SimpleBehaviour(Sliding_Enter, Sliding_Update, Sliding_FixedUpdate, Sliding_Exit));
    }

    private void Sliding_Enter(PlayerState prev)
    {

    }

    private void Sliding_Update()
    {
        UpdateRotation(false);

        if (crashSpeedFactor < slidingSpeed)
        {
            crashSpeedFactor += 1.0f * Time.deltaTime;
        }
        else
        {
            crashSpeedFactor = 1.0f;
        }
    }

    private void Sliding_FixedUpdate()
    {
        float sideSpeed = moveDirectionRaw.x * slidingSideSpeed;

        Vector3 desiredVelocity =
            (slidingForward * slidingSpeed + slidingRight * sideSpeed) * crashSpeedFactor;

        movement.Move(desiredVelocity, slidingSpeed + Mathf.Abs(sideSpeed), acceleration,
            deceleration, groundFriction, groundFriction, !allowVerticalMovement);

        Jump();
    }

    private void Sliding_Exit(PlayerState next)
    {
        slidingSlope = null;
        rotationChanging = false;

        // TODO 카메라 관련 처리
    }
    #endregion


    public void SetSliding(SlidingSlope inSlope)
    {
        if (inSlope == null)
            return;

        slidingSlope = inSlope;
        slidingForward = slidingSlope.transform.forward;
        slidingRight = slidingSlope.transform.right;


        Vector3 lookDirection = Vector3.ProjectOnPlane(slidingForward, Vector3.up);
        ChangeRotation(Quaternion.LookRotation(lookDirection), slidingEnterTime);

        ChangeState(PlayerState.Sliding);


        // TODO 카메라 관련 처리
    }

    public void CrashObstacle(int damage)
    {
        playerCharacter.TakeDamage(damage);

        crashSpeedFactor = 0.0f;
    }
}
