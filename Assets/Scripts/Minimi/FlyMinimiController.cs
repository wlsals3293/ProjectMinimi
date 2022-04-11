using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public class FlyMinimiController : BaseAgentController
{
    [SerializeField]
    private float floatingDistance = 2f;

    [Tooltip("수평이동 지연값. 값이 클수록 목표를 늦게 따라감")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float horizontalDamping = 0.05f;

    [Tooltip("수직이동 지연값. 값이 클수록 목표를 늦게 따라감")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float verticalDamping = 0.2f;


    private Vector3 targetOffset = new Vector3(0, 1.5f, 0.0f);

    private Vector3 targetPosition;

    private Vector3 currentPosition;


    private float xDampVelocity;
    private float yDampVelocity;
    private float zDampVelocity;


    protected override void FixedUpdate()
    {
        // Pause / resume character

        Pause();

        // If paused, return

        if (isPaused)
            return;

        // Perform character movement

        Move();
    }

    public override void Update()
    {
        // If paused, return

        if (isPaused)
            return;

        // Update character rotation (if not paused)

        UpdateRotation();

        // Perform character animation (if not paused)

        Animate();
    }

    protected override void SetMoveDirection()
    {
        // 네비게이션을 사용하고 있지 않으면 리턴
        if (!IsNavMovement)
            return;

        // If agent is not moving, return

        moveDirection = Vector3.zero;

        if (!agent.hasPath)
            return;

        // If destination not reached,
        // feed agent's desired velocity (lateral only) as the character move direction

        if (agent.remainingDistance > stoppingDistance)
            moveDirection = Vector3.ProjectOnPlane(agent.desiredVelocity, transform.up);
        else
        {
            // If destination is reached,
            // reset stop agent and clear its path

            agent.ResetPath();
        }
    }

    protected override Vector3 CalcDesiredVelocity()
    {
        SetMoveDirection();

        var desiredVelocity = base.CalcDesiredVelocity();
        return autoBraking ? desiredVelocity * brakingRatio : desiredVelocity;
    }

    protected override void Move()
    {
        // Apply movement

        // If using root motion and root motion is being applied (eg: grounded),
        // move without acceleration / deceleration, let the animation takes full control

        var desiredVelocity = CalcDesiredVelocity();

        if (useRootMotion && applyRootMotion)
            movement.Move(desiredVelocity, speed, !allowVerticalMovement);
        else
        {
            // Move with acceleration and friction

            var currentFriction = isGrounded ? groundFriction : airFriction;
            var currentBrakingFriction = useBrakingFriction ? brakingFriction : currentFriction;

            movement.Move(desiredVelocity, speed, acceleration, deceleration, currentFriction,
                currentBrakingFriction, !allowVerticalMovement);
        }

        // Update root motion state,
        // should animator root motion be enabled? (eg: is grounded)

        applyRootMotion = useRootMotion && movement.isGrounded;
    }

    protected override void Animate()
    {

    }

    /// <summary>
    /// 목표물 따라가기
    /// </summary>
    private void FollowTarget()
    {
        // 오프셋 적용 (월드 기준)
        Vector3 adjustedPosition = targetPosition + targetOffset;


        currentPosition.x = Mathf.SmoothDamp(
            currentPosition.x, adjustedPosition.x, ref xDampVelocity, horizontalDamping);
        currentPosition.y = Mathf.SmoothDamp(
            currentPosition.y, adjustedPosition.y, ref yDampVelocity, verticalDamping);
        currentPosition.z = Mathf.SmoothDamp(
            currentPosition.z, adjustedPosition.z, ref zDampVelocity, horizontalDamping);

    }
}
