using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Ÿ���� �����Ÿ� �ȿ� �ִ��� Ȯ���մϴ�.")]
    [TaskCategory("Movement")]
    public class TargetInRange : Conditional
    {
        [Tooltip("Ȯ���� Ÿ��.")]
        public SharedGameObject target;

        [Tooltip("�Ÿ�")]
        public SharedFloat distance = 10f;

        [Tooltip("üũ�ϸ� Ÿ���� �����Ÿ� �ۿ� �ִ��� Ȯ���մϴ�.")]
        public bool invert;


        public override TaskStatus OnUpdate()
        {
            if (transform == null || target == null)
                return TaskStatus.Failure;

            Vector3 targetPosition = target.Value.transform.position;

            float separationDistance = (targetPosition - transform.position).magnitude;
            if (separationDistance <= distance.Value)
            {
                return invert ? TaskStatus.Failure : TaskStatus.Success;
            }

            return invert ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            target = null;
            distance = 10f;
            invert = false;
        }
    }
}