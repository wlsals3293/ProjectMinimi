using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("���� �÷��̾� ������Ʈ�� ���� Player ������ ����ֽ��ϴ�.")]
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