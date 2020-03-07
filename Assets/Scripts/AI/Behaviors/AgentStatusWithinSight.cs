using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using GameplayScripts;
// ReSharper disable UnassignedField.Global
// ReSharper disable CheckNamespace

[TaskCategory("Grid System")]
public class AgentStatusWithinSight : Conditional
{
	// ReSharper disable once MemberCanBePrivate.Global
	public SharedFOV myFieldOfView;
	// ReSharper disable once MemberCanBePrivate.Global
	public AgentStatus status;

	public override TaskStatus OnUpdate()
	{
		return myFieldOfView.Value.agentsISee.Any(agent => agent.status == status) ? TaskStatus.Success : TaskStatus.Failure;
	}
}


		
