﻿using System;
using System.Collections;
using System.Collections.Generic;
using GridManagement;
using UnityEngine;

namespace Gameplay
{
	public enum AgentStatus
	{
		Normal,
		Player,
		Dead,
		Stunned,
		Inanimate,
		OnFire,
		Curious
	}

	[RequireComponent(typeof(GridElement))]
	public class Agent : MonoBehaviour
	{
		public AgentStatus status;
		public GridElement gridElement;
		public TilemapManager tilemapManager;
		public Vector2Int position => tilemapManager.WorldToCell(transform.position);
		public Vector2Int destination;
		[HideInInspector] public Queue<Vector2Int> pathToDestination = new Queue<Vector2Int>();
		
		Pathfind pathfind;

		//doing stuff up in this shit.
		public Vector2Int facingDirection;
		public Action atDestinationAction;
		public bool pushable;
		public bool atDestination => destination == position;
		Coroutine _ambientPathfinding;

		protected void Awake()
		{
			pathfind = new Pathfind {tilemapManager = tilemapManager};
			gridElement = GetComponent<GridElement>();
			status = AgentStatus.Normal;
			if (gridElement.solid && pushable)
			{
				Debug.LogWarning("Agent is pushable but gridElement is set to solid. overriding gridElement solid to false");
				gridElement.solid = false;
			}
		}

		private void Start()
		{
			destination = position;
		}

		//moves in a dir out of the movement stack
		public TurnInfo MoveTowardsDestination()
		{
			if (position == destination)
			{
				atDestinationAction?.Invoke();
			}

			Vector2Int next = pathToDestination.Dequeue();
			if (next == position) //get rid of first item, which is ... already where we are.
			{
				next = pathToDestination.Dequeue();
			}

			Vector2Int dir = next - position;
			return Move(dir);
		}
		public TurnInfo Move(Vector2Int dir, bool useUpTurn = true)
		{
			TurnInfo info = new TurnInfo {useUpTurn = useUpTurn};
			return DoMove(dir, info);
		}

		protected virtual void MoveEnded(TurnInfo turn)
		{
			gridElement.OnNewPosition();
		}

		private TurnInfo DoMove(Vector2Int dir, TurnInfo info)
		{
			//tests
			if (dir.magnitude != 1)
			{
				Debug.LogError("invalid movement for move: " + dir, gameObject);
			}

			if (status == AgentStatus.Dead)
			{
				info.turnTaken = false;
				return info;
			}

			List<Agent> pushing = new List<Agent>();
			if (CanMoveInDir(dir, ref pushing))
			{
				info.blockPlayerMovement = true;
				info.endOfMoveAction += MoveEnded;
				facingDirection = dir;
				StartCoroutine(Lerp(transform.position, transform.position + new Vector3(dir.x, dir.y, 0), 0.33f, info));
				foreach (Agent m in pushing)
				{
					m.Move(dir, false); //dont use the move of an agent that gets pushed.                
				}

				gridElement.OnNewPosition();
			}
			else
			{
				info.turnTaken = false;
			}

			return info;
		}

		private IEnumerator Lerp(Vector3 start, Vector3 end, float timeToMove, TurnInfo info)
		{
			float t = 0;
			while (t <= timeToMove)
			{
				transform.position = Vector3.Lerp(start, end, t);
				t = t + Time.deltaTime / timeToMove;
				yield return null;
			}

			transform.position = end;
			info.Finish();
		}

		public bool CanMoveInDir(Vector2Int dir, ref List<Agent> pushing)
		{
			TileNode next = gridElement.tilemapManager.GetTileNode(position + dir);
			if (next == null)
			{
				return false;
			}

			if (next.solid)
			{
				return false;
			}

			List<GridElement> atNode = next.itemsHere;
			foreach (GridElement gridElementIn in next.itemsHere)
			{
				//can we push solid things?
				if (gridElementIn.solid)
				{
					return false;
				}

				if (gridElementIn.GetComponent<Agent>() != null)
				{
					Agent neighbor = gridElementIn.GetComponent<Agent>();
					if (neighbor.pushable && neighbor != this)
					{
						if (neighbor.CanMoveInDir(dir, ref pushing))
						{
							pushing.Add(neighbor);
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}

			return true;
		}

		public Pathfind GetPathfinder()
		{
			return pathfind;
		}
		public GridManagement.Pathfind SetDestination(TileNode destination)
		{
			pathfind.pathStatus = 0;
			StartCoroutine(WaitForPathThenQueueMoves(destination));
			return pathfind;
		}
		public Pathfind SetDestination(Vector3 worldSpacePos)
		{
			var destination = tilemapManager.GetTileNode(tilemapManager.WorldToCell(worldSpacePos));
			pathfind.pathStatus = 0;
			StartCoroutine(WaitForPathThenQueueMoves(destination));
			return pathfind;
		}

		public Pathfind SetDestination(Vector2Int V2destination)
		{
			return SetDestination(gridElement.tilemapManager.GetTileNode(V2destination));
		}

		public IEnumerator WaitForPathThenQueueMoves(TileNode destinationNode)
		{
			destination = destinationNode.position;
			pathfind.Search(gridElement.tileNode, destinationNode, this);
			pathToDestination.Clear();
			bool pathFailed = false;
			while (pathfind.pathStatus != 1)
			{
				if (pathfind.pathStatus == -1)
				{
					pathFailed = true;
					break;
				}

				yield return null;
			}

			if (!pathFailed && pathfind.path != null)
			{
				//do nothing if the path failed to be found
				//sanity check on failed path
				for (int i = 0; i < pathfind.path.Count; i++)
				{
					pathToDestination.Enqueue(pathfind.path[i].position);
				}
			}
		}

		public void CachePathfind()
		{
			if (pathfind.running)
			{
				StopCoroutine(_ambientPathfinding);
			}

			_ambientPathfinding = StartCoroutine(pathfind.FindAllPaths(gridElement.tileNode, 10));
		}
	}
}