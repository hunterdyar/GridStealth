using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using GameplayScripts;
using Inventory;

public class PlayerAgent : Agent
{
	public GameFlowManager gfm;
	public SharedVector2Int sharedPosition;

	void Awake()
	{
		status = AgentStatus.Player;
	}
	protected void Start()
	{
		SharedAgent playerAgent = new SharedAgent();
		playerAgent.Value = this;
		GlobalVariables.Instance.SetVariable("playerAgent", playerAgent);
		sharedPosition.Value = position;
	}

	protected override void MoveEnded(TurnInfo turn)
	{
		sharedPosition.Value = position;
		GlobalVariables.Instance.SetVariable("playerPosition", sharedPosition);
		base.MoveEnded(turn);
		if (turn.useUpTurn)
		{
			gfm.PlayerTookTurn();
		}
	}

	public void WaitATurn()
	{
		gfm.PlayerTookTurn();
	}
}