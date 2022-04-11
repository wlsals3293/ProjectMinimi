using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("현재 플레이어 오브젝트를 얻어와 Player 변수에 집어넣습니다.")]
    [TaskCategory("Initialize")]
    //[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}MoveTowardsIcon.png")]
    public class SetPlayerVariable : Action
    {
        public override TaskStatus OnUpdate()
        {
            if (Owner == null)
                return TaskStatus.Failure;

            GameObject playerObject = PlayerManager.Instance.PlayerChar.gameObject;

            if (playerObject == null)
                return TaskStatus.Failure;

            Owner.SetVariable("Player", (SharedGameObject)playerObject);

            return TaskStatus.Success;
        }
    }
}