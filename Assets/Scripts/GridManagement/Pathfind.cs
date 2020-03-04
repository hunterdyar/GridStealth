using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class Pathfind
{
    public TilemapManager tilemapManager;
    TileNode cachedStart;
    public List<TileNode> path = new List<TileNode>();
    public Dictionary<TileNode,int> distances = new Dictionary<TileNode, int>();
    Dictionary<TileNode,TileNode> cameFrom = new Dictionary<TileNode, TileNode>();
    public int pathStatus = 0;
    public bool running = false;
    public void Search(TileNode start,TileNode end,MonoBehaviour context)
    {

        //start up the search.
        
        //Reset our status if need be. If not need be... we should carry on.
        if(cachedStart != start){pathStatus = 0;}
        //
        if(pathStatus == 0 || pathStatus == -1){//if path is unfound or failed to find

            context.StartCoroutine(FindAllPaths(start,100));//high iteration number basically means "we need it now!"
            //sadly it restarts. Could we have it find a currently running coroutine and change the iteration value? 
            //that would be neat
        }
        TileNode search = end;
        path.Clear();
        while(search != start)
        {
            if(cameFrom.ContainsKey(search))
            {   
                path.Add(search);
                search = cameFrom[search];
            }else{
                pathStatus = -1;
                return;
            }
        }
        path.Add(start);
        path.Reverse();
    }
    public IEnumerator FindAllPaths(TileNode start,int iterationsPerFrame)
    {
        running = true;
        Queue<TileNode> frontier = new Queue<TileNode>();
        cachedStart = start;
        frontier.Enqueue(start);
        cameFrom = new Dictionary<TileNode, TileNode>();
        distances = new Dictionary<TileNode, int>();
        distances[start] = 0;
        int iterations = 0;
        //
       // Debug.Log("pathfinding...");
        while(frontier.Count > 0)
        {
            TileNode current = frontier.Dequeue();
            foreach(TileNode next in tilemapManager.GetConnectionsTo(current))
            {
                if(!distances.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    distances[next] = distances[current]+1;
                    cameFrom[next] = current;
                }
            }
            //performance things
            iterations++;
            if(iterations >= iterationsPerFrame){
                iterations = 0;
                yield return null;
            }
        }
        pathStatus = 1;
        running = false;
    }
}
