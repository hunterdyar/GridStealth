using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Gameplay;

// ReSharper disable UnassignedField.Global

[TaskCategory("Grid System")]
public class AtDestination : Conditional
{
	// ReSharper disable once MemberCanBePrivate.Global
	public SharedAgent myAgent;
	// ReSharper disable once MemberCanBePrivate.Global
	public AgentStatus status;
	
	public override TaskStatus OnUpdate()
	{
		return (myAgent.Value.atDestination) ? TaskStatus.Success : TaskStatus.Failure;
	}
}


		
