using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(GridElement))]
public class Agent : MonoBehaviour
{
    [HideInInspector]
    public GridElement gridElement;
    public TilemapManager tilemapManager;
    public Vector2Int position {get{return gridElement.position;}}
    public Vector2Int destination;
    public Queue<Vector2Int> pathToDestination = new Queue<Vector2Int>(); 
    Pathfind pathfind;
    public Vector2Int facingDirection;
    public Action atDestinationAction;
    public bool pushable;
    Coroutine ambientPathfinding;
    void Awake()
    {
        pathfind = new Pathfind();
        pathfind.tilemapManager = tilemapManager;
        gridElement = GetComponent<GridElement>();
        if(gridElement.solid && pushable)
        {
            Debug.LogWarning("Agent is pushable but gridElement is set to solid. overriding gridElement solid to false");
            gridElement.solid = false;
        }
        destination = position;
    }
    //moves in a dir out of the movement stack
    public TurnInfo MoveTowardsDestination()
    {
        if(position == destination)
        {
            atDestinationAction?.Invoke();
        }
        Vector2Int next = pathToDestination.Dequeue();
        Vector2Int dir = next-position;
        return Move(dir);
    }
    public TurnInfo Move(Vector2Int dir)
    {
        TurnInfo info = new TurnInfo();
        return DoMove(dir,info);
    }
    protected virtual void MoveEnded()
    {
        gridElement.OnNewPosition();
    }

    public TurnInfo DoMove(Vector2Int dir, TurnInfo info)
    {
        //tests
        if(dir.magnitude != 1){Debug.LogError("invalid movement for move: "+dir,gameObject);}
        List<Agent> pushing = new List<Agent>();
        if(CanMoveInDir(dir, ref pushing))
        {
            info.blockPlayerMovement = true;
            info.endOfMoveAction += MoveEnded;
            facingDirection = dir;
            StartCoroutine(Lerp(transform.position,transform.position+new Vector3(dir.x,dir.y,0),0.33f, info));
            foreach(Agent m in pushing)
            {
                m.Move(dir);                
            }
            gridElement.OnNewPosition();
        }
        return info;
    }
    public IEnumerator Lerp(Vector3 start,Vector3 end, float timeToMove, TurnInfo info)
    {
        float t = 0;
        while(t<=timeToMove)
        {
            transform.position = Vector3.Lerp(start,end,t);
            t = t+Time.deltaTime/timeToMove;
            yield return null;
        }
        transform.position = end;
        info.Finish();
    }
    public bool CanMoveInDir(Vector2Int dir,ref List<Agent> pushing)
    {
        TileNode next = gridElement.tilemapManager.GetTileNode(position+dir);
        if(next == null){return false;}
        if(next.solid){return false;}
        List<GridElement> atNode = next.itemsHere;
        foreach(GridElement gein in next.itemsHere)
        {
            //can we push solid things?
            if(gein.solid){return false;}
            if(gein.GetComponent<Agent>() != null)
            {
                Agent neigh = gein.GetComponent<Agent>();
                if(neigh.pushable && neigh != this){
                    if(neigh.CanMoveInDir(dir,ref pushing))
                    {
                        pushing.Add(neigh);
                    }else{
                        return false;
                    }
                }else{
                    return false;
                }
            }
        }
        return true;
    }
    public Pathfind SetDestination(TileNode destination)
    {
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
        pathfind.Search(gridElement.tileNode,destinationNode,this);
        pathToDestination.Clear();
        bool pathFailed = false;
        while(pathfind.pathStatus != 1){
            if(pathfind.pathStatus == -1){
                pathFailed = true;
                break;
            }
            yield return null;
        }
        if(!pathFailed){//do nothing if the path failed to be found
            if(pathfind.path != null){//sanity check on failed path
                for(int i = 0;i<pathfind.path.Count;i++)
                {
                    pathToDestination.Enqueue(pathfind.path[i].position);
                }
            }
        }
    }
    
    public void CachePathfind()
    {
        if(pathfind.running)
        {
            StopCoroutine(ambientPathfinding);
        }
        ambientPathfinding = StartCoroutine(pathfind.FindAllPaths(gridElement.tileNode,10));
    }
}
