﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridManagement
{
	public class Pathfind
	{
		internal TilemapManager tilemapManager;
		TileNode _cachedStart;
		public readonly List<TileNode> path = new List<TileNode>();
		public Dictionary<TileNode, int> Distances { get; set; } = new Dictionary<TileNode, int>();
		Dictionary<TileNode, TileNode> _cameFrom = new Dictionary<TileNode, TileNode>();
		public int pathStatus;
		public bool running;

		public void Search(TileNode start, TileNode end, MonoBehaviour context)
		{
			//start up the search.
			//Reset our status if need be. If not need be... we should carry on.
			if (_cachedStart != start)
			{
				pathStatus = 0;
			}

			if (pathStatus == 0 || pathStatus == -1)
			{
				//if path is unfound or failed to find

				context.StartCoroutine(FindAllPaths(start, 100)); //high iteration number basically means "we need it now!"
				//sadly it restarts. Could we have it find a currently running coroutine and change the iteration value? 
				//that would be neat
			}

			var search = end;
			path.Clear();
			while (search != start)
			{
				if (_cameFrom.ContainsKey(search))
				{
					path.Add(search);
					search = _cameFrom[search];
				}
				else
				{
					pathStatus = -1;
					return;
				}
			}

			path.Add(start);
			path.Reverse();
		}

		public IEnumerator FindAllPaths(TileNode start, int iterationsPerFrame)
		{
			running = true;
			var frontier = new Queue<TileNode>();
			_cachedStart = start;
			frontier.Enqueue(start);
			_cameFrom = new Dictionary<TileNode, TileNode>();
			Distances = new Dictionary<TileNode, int> {[start] = 0};
			var iterations = 0;
			// Debug.Log("pathfinding...");
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				foreach (var next in tilemapManager.GetConnectionsTo(current))
				{
					if (Distances.ContainsKey(next))
					{
						continue;
					}

					frontier.Enqueue(next);
					Distances[next] = Distances[current] + 1;
					_cameFrom[next] = current;
				}

				//performance things
				iterations++;
				// ReSharper disable once InvertIf
				if (iterations >= iterationsPerFrame)
				{
					iterations = 0;
					yield return null;
				}
			}

			pathStatus = 1;
			running = false;
		}
	}
}