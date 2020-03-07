using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(GridElement))]
	public class FOV : MonoBehaviour
	{
		GridElement _gridElement;
		Agent _agent;
		GameFlowManager _gfm;
		public int viewRange;
		public bool canSee = true;
		public List<GridElement> itemsISee;
		public List<Agent> agentsISee;
		public List<Vector2Int> placesISee;
		public FloatReference visibilityThreshold;

		[Button("Assign gridElement")]
		void Awake()
		{
			agentsISee = new List<Agent>();
			itemsISee = new List<GridElement>();
			placesISee = new List<Vector2Int>();
			_agent = GetComponent<Agent>();
			_gridElement = GetComponent<GridElement>();
			//the WithinSight behavior will run CheckSight before ... checking the sight...
			//gridElement.OnNewPositionAction += CheckSight;
		}

		void OnEnable()
		{
			//gfm.PostGridElementsUpdatedAction += CheckSight;
		}

		void OnDisable()
		{
			//gfm.PostGridElementsUpdatedAction -= CheckSight;
		}

		[Button("Illuminate")]
		public void CheckSight()
		{
			placesISee.Clear();
			agentsISee.Clear();
			itemsISee.Clear();
			if (!canSee)
			{
				return;
			}

			var c = GridUtility.Arc(_gridElement.position, _agent.facingDirection, viewRange, 45);
			foreach (var p in c)
			{
				var tn = _gridElement.tilemapManager.GetTileNode(p);
				if (tn != null)
				{
					if (tn.brightness > visibilityThreshold.Value)
					{
						if (_gridElement.tilemapManager.LineOfSight(tn.position, _gridElement.position))
						{
							foreach (GridElement item in tn.itemsHere)
							{
								placesISee.Add(tn.position);
								if (!itemsISee.Contains(item))
								{
									itemsISee.Add(item);
									if (item.GetComponent<Agent>() != null)
									{
										agentsISee.Add(item.GetComponent<Agent>());
									}
								}
							}
						}
					}
				}
			}
		}
	}
}