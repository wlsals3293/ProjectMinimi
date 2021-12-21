using UnityEngine;
using UnityEngine.AI;
using ECM.Controllers;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("타겟을 따라다닙니다.")]
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FollowIcon.png")]
    public class FollowAnywhere : Action
    {
        [Tooltip("The GameObject that the agent is following")]
        public SharedGameObject target;
        [Tooltip("Start moving towards the target if the target is further than the specified distance")]
        public SharedFloat moveDistance = 2;

        private Vector3 lastTargetPosition;
        private bool hasMoved;

        // Component references
        private BaseAgentController controller;
        private NavMeshAgent agent;


        public override void OnAwake()
        {
            controller = GetComponent<BaseAgentController>();
            if (controller != null)
                agent = controller.agent;
        }

        public override void OnStart()
        {
            if (target.Value == null || controller == null)
            {
                return;
            }

            lastTargetPosition = target.Value.transform.position + Vector3.one * (moveDistance.Value + 1);
            hasMoved = false;

        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null || controller == null)
            {
                return TaskStatus.Failure;
            }

            var targetPosition = target.Value.transform.position;

            if (controller.IsNavMovement)
            {
                // Move if the target has moved more than the moveDistance since the last time the agent moved.
                if ((targetPosition - lastTargetPosition).magnitude >= moveDistance.Value)
                {
                    agent.isStopped = false;
                    agent.SetDestination(targetPosition);

                    lastTargetPosition = targetPosition;
                    hasMoved = true;
                }
                else
                {
                    // Stop moving if the agent is within the moveDistance of the target.
                    if (hasMoved && (targetPosition - transform.position).magnitude < moveDistance.Value)
                    {
                        agent.isStopped = true;
                        hasMoved = false;
                        lastTargetPosition = targetPosition;
                    }
                }
            }
            else
            {
                Vector3 moveVector = targetPosition - transform.position;
                // Move if the target has moved more than the moveDistance since the last time the agent moved.
                if (moveVector.magnitude >= moveDistance.Value)
                {
                    controller.moveDirection = Vector3.ProjectOnPlane(moveVector, transform.up).normalized;

                    lastTargetPosition = targetPosition;
                    hasMoved = true;
                }
                else
                {
                    // Stop moving if the agent is within the moveDistance of the target.
                    if (hasMoved && (targetPosition - transform.position).magnitude < moveDistance.Value)
                    {
                        controller.moveDirection = Vector3.zero;

                        hasMoved = false;
                        lastTargetPosition = targetPosition;
                    }
                }

                // 네비게이션을 이용하지 않고 있을 때, 
                // 에이전트의 위치가 네비게이션 안에 있으면 네비게이션을 이용하도록 설정
                if (controller.CheckOnNavMesh())
                {
                    if (agent.Warp(transform.position))
                        controller.IsNavMovement = true;
                }
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            moveDistance = 2;
        }
    }
}