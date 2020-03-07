using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AIPatrolAB : AIBase
{
	public Transform pointA;
	public Transform pointB;
	Vector2Int a;
	Vector2Int b;
	Vector2Int mostRecent;

	new void Awake()
	{
		base.Awake();
		agent = GetComponent<Agent>();
		SetPositions();
		mostRecent = a;
	}

	void SetPositions()
	{
		a = gridElement.tilemapManager.WorldToCell(pointA.position);
		b = gridElement.tilemapManager.WorldToCell(pointB.position);
	}

	Vector2Int GetDirection()
	{
		Vector2Int dir = Vector2Int.zero;
		if (mostRecent == a)
		{
			dir = gridElement.position - b;
			if (dir == Vector2Int.zero)
			{
				mostRecent = b;
				return GetDirection();
			}
		}
		else if (mostRecent == b)
		{
			dir = gridElement.position - a;
			if (dir == Vector2Int.zero)
			{
				mostRecent = a;
				return GetDirection();
			}
		}

		return dir;
	}

	public override TurnInfo TakeTurn()
	{
		SetPositions();
		Vector2Int dir = GetDirection();


		dir = new Vector2Int((int) Mathf.Clamp01(dir.x), (int) Mathf.Clamp01(dir.y));
		Debug.Log(dir);
		TurnInfo info = new TurnInfo(this);
		return agent.Move(dir);
	}
}