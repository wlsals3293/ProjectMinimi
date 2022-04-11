using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ECM.Controllers;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Å¸°ÙÀ» µû¶ó´Ù´Õ´Ï´Ù.")]
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
