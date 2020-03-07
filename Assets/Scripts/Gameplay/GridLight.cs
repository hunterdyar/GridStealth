using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
	public enum GridLightType
	{
		Circle,
		Arc
	}

	public enum GridLightCuriosity
	{
		Normal,
		Curious,
		VeryCurious,
	}

	[RequireComponent(typeof(GridElement))]
	public class GridLight : MonoBehaviour
	{
		Vector2Int position => gridElement.position;
		GridElement gridElement;
		Agent agent;
		public AnimationCurve lightFalloffCurve;
		public GameFlowManager gfm;
		public GridLightType gridLightType;
		public int lightRange;
		public bool lightOn = true;

		[Button("Assign gridElement")]
		void Awake()
		{
			gridElement = GetComponent<GridElement>();
			agent = GetComponent<Agent>();
			gridElement.onNewPositionAction += Illuminate;
		}

		void OnEnable()
		{
			gfm.postGridElementsUpdatedAction += Illuminate;
		}

		void OnDisable()
		{
			gfm.postGridElementsUpdatedAction -= Illuminate;
		}

		public void ForceIlluminate()
		{
			Illuminate();
		}

		[Button("Illuminate")]
		void Illuminate()
		{
			

			//remove self from all levelNodes.
			foreach (TileNode t in gridElement.tilemapManager.allTileNodes)
			{
				if (t.lightsOnMe.Contains(this))
				{
					t.lightsOnMe.Remove(this);
					t.SetBrightness();
				}
			}
			//dont do anything if we are turned off.
			if (!lightOn)
			{
				return;
			}

			var c = new Vector2Int[0];
			switch (gridLightType)
			{
				case GridLightType.Circle:
					c = GridUtility.Circle(position, lightRange);
					break;
				case GridLightType.Arc:
					c = GridUtility.Arc(position, agent.facingDirection, lightRange, 45);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			foreach (Vector2Int p in c)
			{
				TileNode tn = gridElement.tilemapManager.GetTileNode(p);
				if (tn != null)
				{
					if (gridElement.tilemapManager.LineOfSight(tn.position, position))
					{
						if (!tn.lightsOnMe.Contains(this))
						{
							tn.lightsOnMe.Add(this);
							tn.SetBrightness();
						}
					}
				}
			}

			gridElement.tilemapManager.UpdateBrightnessDisplay();
		}

		public float BrightnessForTile(Vector2Int tilePos)
		{
			return (lightFalloffCurve.Evaluate((lightRange - GridUtility.ManhattanDistance(position, tilePos)) / (float) lightRange));
		}

		void SetBrightnessOfNeighbors(TileNode starting, int depth, int maxDepth, ref List<Vector2Int> checkedN)
		{
			checkedN.Add(starting.position);
			List<TileNode> ns = new List<TileNode>(gridElement.tilemapManager.GetNeighborsTo(starting));
			foreach (TileNode n in ns)
			{
				if (depth <= maxDepth)
				{
					if (!checkedN.Contains(n.position))
					{
						// if(holdingGrid.LineOfSight(this,n))
						// {
						n.brightness = gridElement.brightness - GridUtility.ManhattanDistance(position, n.position);
						checkedN.Add(n.position);
						//Recursion!
						SetBrightnessOfNeighbors(n, depth + 1, maxDepth, ref checkedN);
						// }else{Debug.Log("no line of sight,"+n.position);}
					}
					else
					{
						Debug.Log("already checked, " + n.position);
					}
				}
				else
				{
					Debug.Log("max depth past, " + depth + ", " + n.position);
				}
			}
		}
	}
}