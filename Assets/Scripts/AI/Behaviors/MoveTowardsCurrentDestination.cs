using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameplayScripts;
using GridManagement;

[TaskCategory("Grid System")]
public class MoveTowardsCurrentDestination : Action
{
	public SharedAgent myAgent;
	bool pathFailed = false;
	Pathfind finder;

	public override TaskStatus OnUpdate()
	{
		finder = myAgent.Value.GetPathfinder();

		if (myAgent.Value.atDestination)
		{
			return TaskStatus.Success;
		}

		if (pathFailed)
		{
			return TaskStatus.Failure;
		}

		StartCoroutine(MoveTowardsDestination());
		return TaskStatus.Running;
	}

	IEnumerator MoveTowardsDestination()
	{
		bool pathfailed = false;
		while (finder.pathStatus != 1)
		{
			if (finder.pathStatus == -1)
			{
				pathfailed = true;
				break;
			}

			yield return null;
		}

		if (!pathfailed)
		{
			while (myAgent.Value.pathToDestination.Count < 0)
			{
				yield return null;
			}

			myAgent.Value.MoveTowardsDestination();
		}
	}
}