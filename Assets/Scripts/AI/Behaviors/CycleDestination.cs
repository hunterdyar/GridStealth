using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GridManagement;
// ReSharper disable MemberCanBePrivate.Global

[TaskCategory("Grid System")]
public class CycleDestination : Action
{
	public Transform[] destinations;
	public int currentDestination;
	public SharedAgent myAgent;
	
	

	public override TaskStatus OnUpdate()
	{
		currentDestination++;
		//cycle
		if (currentDestination == destinations.Length)
		{
			currentDestination = 0;
		}

		myAgent.Value.SetDestination(destinations[currentDestination].position);
		return TaskStatus.Success;
	}
}