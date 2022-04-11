using UnityEngine;
using UnityEngine.AI;
using ECM.Controllers;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Ÿ�� ��ó�� �����̵� �մϴ�.")]
    [TaskCategory("Movement")]
    public class TeleportToTarget : Action
    {
        [Tooltip("�����̵��� Ÿ��")]
        public SharedGameObject target;

        // Component references
        private BaseAgentController controller;
        private NavMeshAgent agent;


        public override void OnAwake()
        {
            controller = GetComponent<BaseAgentController>();
            if (controller != null)
                agent = controller.agent;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null || controller == null)
            {
                return TaskStatus.Failure;
            }

            var targetPosition = target.Value.transform.position;

            Vector3 teleportPos = targetPosition - target.Value.transform.forward * 1.5f;

            if (controller.CheckOnNavMesh(teleportPos))
            {
                agent.Warp(teleportPos);
                controller.IsNavMovement = true;
            }
            else
            {
                transform.position = teleportPos;
                controller.IsNavMovement = false;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
        }
    }
}