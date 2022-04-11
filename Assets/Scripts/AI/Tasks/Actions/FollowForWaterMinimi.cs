using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ECM.Controllers;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Ÿ���� ����ٴմϴ�.")]
	[TaskCategory("Movement")]
	public class FollowForWaterMinimi : Action
	{
		[Tooltip("The GameObject that the agent is following")]
		public SharedGameObject target;


		private BaseAgentController controller;


		public override void OnStart()
		{

		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
	}
}
