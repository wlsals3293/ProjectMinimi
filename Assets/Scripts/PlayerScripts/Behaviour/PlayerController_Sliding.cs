using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;


public partial class PlayerController : BaseCharacterController
{
    [Header("Sliding")]

    [SerializeField] private float slidingSpeed = 20.0f;
    private float slidingEnterTime = 0.2f;

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
        UpdateRotationChanging();
    }

    private void Sliding_FixedUpdate()
    {
        Vector3 desiredVelocity = 
            (slidingForward + slidingRight * moveDirection.x).normalized * slidingSpeed;

        movement.Move(desiredVelocity, slidingSpeed, !allowVerticalMovement);

        Jump();
    }

    private void Sliding_Exit(PlayerState next)
    {
        slidingSlope = null;
        rotationChanging = false;
    }
    #endregion


    public void SetSliding(SlidingSlope inSlope)
    {
        if (inSlope == null)
            return;

        slidingSlope = inSlope;
        slidingForward = Vector3.ProjectOnPlane(slidingSlope.transform.forward, Vector3.up).normalized;
        slidingRight = slidingSlope.transform.right;

        ChangeRotation(Quaternion.LookRotation(slidingForward), slidingEnterTime);
        ChangeState(PlayerState.Sliding);
    }

}
