using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BehaviorDesigner.Runtime;
using Gameplay;

[RequireComponent(typeof(GridElement))]
public class AIBase : MonoBehaviour, IComparable<AIBase>
{
	public int turnsCanTake = 1;
	public GameFlowManager gameFlowManager;
	protected Agent agent;
	public GridElement gridElement;
	BehaviorTree tree;

	protected void Awake()
	{
		agent = GetComponent<Agent>();
		tree = GetComponent<BehaviorTree>();
		gridElement = GetComponent<GridElement>();
		if (turnsCanTake == 0)
		{
			Debug.LogWarning("AI wont take turn, turnsCanTake set to 0.", gameObject);
		}
	}

	public bool GetIsDead()
	{
		if (agent == null)
		{
			//inanimate things cant die?
			//fuck, can we break them tho?
			//need to move status to AIBase.
			return false;
		}
		return agent.status == AgentStatus.Dead;
	}
	public void KillMe()
	{
		agent.status = AgentStatus.Dead;
		GetComponent<SpriteRenderer>().color = Color.grey;
	}
	void Start()
	{
		//Update Behavior Tree Variables
		if (GetComponent<BehaviorTree>() != null)
		{
			var myAgentVar = (SharedAgent) tree.GetVariable("myAgent");
			myAgentVar.Value = agent;
			var myGridElement = (SharedGridElement) tree.GetVariable("myGridElement");
			myGridElement.Value = gridElement;
			var myFOV = (SharedFOV) tree.GetVariable("myFOV");
			myFOV.Value = GetComponent<FOV>();
		}
	}

	public void OnEnable()
	{
		gridElement = GetComponent<GridElement>();
		gameFlowManager.RegisterAI((AIBase) this);
	}

	public void OnDisable()
	{
		gameFlowManager.DeRegisterAI((AIBase) this);
	}

	public virtual TurnInfo TakeTurn()
	{
		if (tree != null)
		{
			var turnsCanTakeVar = (SharedInt) tree.GetVariable("turnsCanTake");
			turnsCanTakeVar.Value = turnsCanTake;

			BehaviorManager.instance.Tick();
		}

		var info = new TurnInfo(this) {turnInMotion = false, blockPlayerMovement = false, turnTaken = false};
		return info;
	}

	public int CompareTo(AIBase comparePart)
	{
		// A null value means that this object is greater.
		if (comparePart == null)
		{
			return 1;
		}
		else
		{
			//x L to R
			if (comparePart.gridElement.position.x < gridElement.position.x)
			{
				return 1;
			}
			else if (comparePart.gridElement.position.x > gridElement.position.x)
			{
				return -1;
			}
			else
			{
				//y for x tie, Top to Bottom
				if (comparePart.gridElement.position.y > gridElement.position.x)
				{
					return 1;
				}
				else if (comparePart.gridElement.position.y < gridElement.position.x)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			}
		}
	}
}