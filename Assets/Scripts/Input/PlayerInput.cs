using System.Collections;
using System.Collections.Generic;
using GameplayScripts;
using GridManagement;
using ScriptableObjectArchitecture;
using UnityEngine;

	// ReSharper disable once CheckNamespace
	[RequireComponent(typeof(Agent))]
	public class PlayerInput : MonoBehaviour
	{
		TilemapManager _tilemapManager;
		Agent _agent;
		public GameFlowManager gfm;
		TurnInfo _activeTurn = new TurnInfo();
		public Queue<Vector2Int> inputStack = new Queue<Vector2Int>();
		Pathfind _pathfind;
		Coroutine _ambientPathfinding;
		public BoolReference handleInput;
		void Awake()
		{
			_tilemapManager = gfm.tilemapManager;
			_pathfind = new Pathfind {tilemapManager = _tilemapManager};
			_agent = GetComponent<Agent>();
		}

		void OnEnable()
		{
			gfm.playerInNewLocationAction += _agent.CachePathfind;
		}

		void OnDisable()
		{
			gfm.playerInNewLocationAction -= _agent.CachePathfind;
		}

		public void GoNorth()
		{
			inputStack.Enqueue(Vector2Int.up);
		}
		public void GoSouth()
		{
			inputStack.Enqueue(Vector2Int.down);
		}
		public void GoEast()
		{
			inputStack.Enqueue(Vector2Int.right);
		}
		public void GoWest()
		{
			inputStack.Enqueue(Vector2Int.left);
		}

		public void EnablePlayerInput()
		{
			handleInput.Value = true;
		}

		public void DisablePlayerInput()
		{
			handleInput.Value = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (_agent.status == AgentStatus.Dead)
			{
				return;
				
			}
			if (!handleInput.Value)
			{
				return;
				
			}
			if (!gfm.playerCanMove.Value)
			{
				return;
			}
			
			//keyboard Input
			if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				inputStack.Enqueue(Vector2Int.right);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				inputStack.Enqueue(Vector2Int.left);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				inputStack.Enqueue(Vector2Int.up);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
			{
				inputStack.Enqueue(Vector2Int.down);
			}

			// if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			// {
			// 	_tilemapManager.Sound(_agent.position, 3);
			// }

			//pathfinding test.
			if (UnityEngine.Input.GetMouseButtonDown(1))
			{
				var clickPos = _tilemapManager.WorldToCell(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
				var clickNode = _tilemapManager.GetTileNode(clickPos);
				if (clickNode == null)
				{
					return;
				}

				StartCoroutine(WaitAndQueuePath(clickNode.position));
			}

			///
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				//if input for movement...
				var clickPos = _tilemapManager.WorldToCell(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));

				if (_agent.position == clickPos)
				{
					return;
				} //if we clicked ON the player. This how we show menu?

				if (_agent.position.x == clickPos.x)
				{
					inputStack.Enqueue(_agent.position.y < clickPos.y ? Vector2Int.up : Vector2Int.down);
				}
				else if (_agent.position.y == clickPos.y)
				{
					inputStack.Enqueue(_agent.position.x < clickPos.x ? Vector2Int.right : Vector2Int.left);
				}
			} //end mouse movement

			///
			if (!_activeTurn.blockPlayerMovement && inputStack.Count > 0)
			{
				_activeTurn = _agent.Move(inputStack.Dequeue());
				if (_activeTurn.turnTaken)
				{
					_activeTurn.blockPlayerMovement = true;
				}
			}
		}

		IEnumerator WaitAndQueuePath(Vector2Int destination)
		{
			bool pathfailed = false;
			var finder = _agent.SetDestination(destination);
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
				while (_agent.pathToDestination.Count < 0)
				{
					yield return null;
				}

				int steps = Mathf.Min(gfm.playerTurnsAllowed.Value - gfm.playerTurnsTaken.Value, _agent.pathToDestination.Count - 1); //-1 is because first item in the queue should be current location, I think.
				var current = _agent.position;
				for (int i = 0; i < steps + 1; i++)
				{
					var next = _agent.pathToDestination.Dequeue();
					if (current != next)
					{
						inputStack.Enqueue(next - current);
						current = next;
					}
				}
			}
		}
	}
