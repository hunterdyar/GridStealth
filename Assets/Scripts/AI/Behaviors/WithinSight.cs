using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Gameplay;

[TaskCategory("Grid System")]
public class WithinSight : Conditional
{
	public SharedFOV myFieldOfView;
	public SharedAgent targetAgent;
	public SharedGridElement targetGE;
	public Vector2Int targetPosition;


	//priority is decending order of specicifity. agent>gridelement>position
	public override TaskStatus OnUpdate()
	{
		//"just in time" updating, instead of subscribing the FOV to actions.
		if (myFieldOfView.Value == null)
		{
			Debug.LogError("WithinSight behavior conditional needs a FOV");
		}

		myFieldOfView.Value.CheckSight();
		//
		if (targetAgent.Value != null)
		{
			if (AgentWithinViewCone(targetAgent.Value))
			{
				return TaskStatus.Success;
			}
			else
			{
				return TaskStatus.Failure;
			}
		}

		if (targetGE.Value != null)
		{
			if (ItemWithinViewCone(targetGE.Value))
			{
				return TaskStatus.Success;
			}
			else
			{
				return TaskStatus.Failure;
			}
		}

		if (PositionWithinViewCone(targetPosition))
		{
			return TaskStatus.Success;
		}

		return TaskStatus.Failure;
	}

	public bool ItemWithinViewCone(GridElement item)
	{
		return (myFieldOfView.Value.itemsISee.Contains(item));
	}

	public bool PositionWithinViewCone(Vector2Int pos)
	{
		return myFieldOfView.Value.placesISee.Contains(pos);
	}

	public bool AgentWithinViewCone(Agent agent)
	{
		return myFieldOfView.Value.agentsISee.Contains(agent);
	}
}