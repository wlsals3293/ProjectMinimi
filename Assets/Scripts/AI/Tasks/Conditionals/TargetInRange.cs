using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("타겟이 일정거리 안에 있는지 확인합니다.")]
    [TaskCategory("Movement")]
    public class TargetInRange : Conditional
    {
        [Tooltip("확인할 타겟.")]
        public SharedGameObject target;

        [Tooltip("거리")]
        public SharedFloat distance = 10f;

        [Tooltip("체크하면 타겟이 일정거리 밖에 있는지 확인합니다.")]
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