using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Gameplay;
using GridManagement;

[TaskCategory("Grid System")]
public class MoveTowards : Action
{
	public SharedVector2Int destination;
	public SharedAgent myAgent;
	Agent agent;
	bool pathFailed = false;
	Pathfind finder;

	public override TaskStatus OnUpdate()
	{
		agent = myAgent.Value;
		finder = agent.SetDestination(destination.Value);

		if (agent.atDestination)
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
			while (agent.pathToDestination.Count < 0)
			{
				yield return null;
			}

			agent.MoveTowardsDestination();
		}
	}
}