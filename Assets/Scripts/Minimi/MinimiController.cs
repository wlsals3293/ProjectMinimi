using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ECM.Controllers;

public class MinimiController : BaseAgentController
{

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

    
}
