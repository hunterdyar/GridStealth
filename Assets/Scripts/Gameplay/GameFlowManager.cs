using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class GameFlowManager : ScriptableObject
	{
		public IntReference startingPlayerTurnsAllowed;
		public IntReference playerTurnsAllowed;
		public IntReference playerTurnsTaken;
		public FloatReference enemySubTurnDelay;
		private int playerTurnsLeft => playerTurnsAllowed.Value - playerTurnsTaken.Value;
		private int _aiTurnsTaken;
		public GameFlowManagerSetup runner;
		public static GameFlowManager instance;
		public TilemapManager tilemapManager;
		[FormerlySerializedAs("PostGridElementsUpdatedAction")] public Action postGridElementsUpdatedAction;
		[FormerlySerializedAs("PlayerInNewLocationAction")] public Action playerInNewLocationAction; //fires before enemies move.
		public BoolReference playerCanMove; //waiting for player input.
		readonly List<AIBase> _lumpAi = new List<AIBase>();

		[Button("Set Singleton")]
		public void SetSingleton()
		{
			instance = this;
		}

		public void Init()
		{
			playerCanMove.Value = true;
			playerTurnsTaken.Value = 0;
			playerTurnsAllowed.Value = startingPlayerTurnsAllowed.Value;
		}

		public void PlayerTookTurn()
		{
			playerTurnsTaken.Value++;
			UpdateLights();
			if (playerTurnsLeft > 0)
			{
				return; //not the AI turn yet.
			}

			playerCanMove.Value = false;
			playerInNewLocationAction?.Invoke();
			//AIAgents to take their turn
			playerTurnsTaken.Value = 0;
			StartAITurn(1);

			//Update Lights, things that care that objects have moved.

			//
			// playerCanMove = true;
		}

		void UpdateLights()
		{
			postGridElementsUpdatedAction?.Invoke(); //i remembered the null operator for once
			tilemapManager.UpdateBrightnessDisplay();
		}

		void StartAITurn(int subTurn)
		{
			runner.StartCoroutine(AIDoMove(subTurn));
		}

		IEnumerator AIDoMove(int subTurn)
		{
			SortAI();
			var playerBlockingTurns = new List<TurnInfo>();
			foreach (AIBase ai in _lumpAi)
			{
				if (subTurn <= ai.turnsCanTake)
				{
					var info = ai.TakeTurn(); //this takes the actual turn
					if (info.blockPlayerMovement)
					{
						playerBlockingTurns.Add(info);
					}

					while (info.blockAIMovement)
					{
						yield return null;
					}
				}
			}
			//all the ai have started their turn coroutines before this wait is called, so they should animate at the same time.

			//This loop wont finish until every player blocking turninfo is false.
			foreach (var pbt in playerBlockingTurns)
			{
				while (pbt.blockPlayerMovement || pbt.blockAIMovement)
				{
					yield return null;
				}
			}

			//update lights
			UpdateLights();


			//thats one turn down. Now lets look towards the next, if there should be one.

			bool takeAnotherAITurn = false;
			foreach (AIBase ai in _lumpAi)
			{
				if (subTurn < ai.turnsCanTake)
				{
					takeAnotherAITurn = true;
					break;
				}
			}

			//
			//AI go again
			if (takeAnotherAITurn)
			{
				yield return new WaitForSeconds(enemySubTurnDelay.Value);
				StartAITurn(subTurn + 1);
			}
			else
			{
				//if they dont go again, its players turn
				playerCanMove.Value = true;
			}
		}

		public void RegisterAI(AIBase ai)
		{
			_lumpAi.Add(ai);
		}

		public void DeRegisterAI(AIBase ai)
		{
			if (_lumpAi.Contains(ai))
			{
				_lumpAi.Remove(ai);
			}
		}

		[Button("sort AI")]
		void SortAI()
		{
			_lumpAi.Sort();
		}

		//
	}
}