using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Agent))]
public class PlayerInput : MonoBehaviour
{
    TilemapManager tilemapManager;
    Agent agent;
    public GameFlowManager gfm;
    TurnInfo activeTurn = new TurnInfo();
    public Queue<Vector2Int> inputStack = new Queue<Vector2Int>();
    Pathfind pathfind;
    Coroutine ambientPathfinding;
    void Awake()
    {
        tilemapManager = gfm.tilemapManager;
        pathfind = new Pathfind();
        pathfind.tilemapManager = tilemapManager;
        agent = GetComponent<Agent>();     
    }
    void OnEnable()
    {
        gfm.PlayerInNewLocationAction += agent.CachePathfind;
    }
    void OnDisable()
    {
        gfm.PlayerInNewLocationAction -= agent.CachePathfind;
    }
    // Update is called once per frame
    void Update()
    {
        if(!gfm.playerCanMove.Value){return;}
        //keyboard Input
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputStack.Enqueue(Vector2Int.right);
        }else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputStack.Enqueue(Vector2Int.left);
        }else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputStack.Enqueue(Vector2Int.up);
        }else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputStack.Enqueue(Vector2Int.down);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            tilemapManager.Sound(agent.position,3);
        }
        ///mouse Input
        
        //pathfinding test.
        if(Input.GetMouseButtonDown(1))
        {
            Vector2Int clickPos = tilemapManager.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            TileNode clickNode = tilemapManager.GetTileNode(clickPos);
            if(clickNode == null){return;}

            StartCoroutine(WaitAndQueuePath(clickNode.position));
        }
        ///
        if(Input.GetMouseButtonDown(0))
        {
            //if input for movement...
            Vector2Int clickPos = tilemapManager.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            if(agent.position == clickPos){return;}//if we clicked ON the player. This how we show menu?
            if(agent.position.x == clickPos.x)
            {
                if(agent.position.y < clickPos.y)
                {
                    inputStack.Enqueue(Vector2Int.up);
                }else
                {
                    inputStack.Enqueue(Vector2Int.down);
                }
            }else if(agent.position.y == clickPos.y)
            {
                if(agent.position.x < clickPos.x)
                {
                    inputStack.Enqueue(Vector2Int.right);
                }else
                {
                    inputStack.Enqueue(Vector2Int.left);
                }
            }
        }//end mouse movement
        
        ///
        if(!activeTurn.blockPlayerMovement && inputStack.Count > 0)
        {
            activeTurn = agent.Move(inputStack.Dequeue());
            activeTurn.blockPlayerMovement = true;
        }


        // ///TESTING

        if(Input.GetKeyDown(KeyCode.A)){
            Vector2Int[] view = GridUtility.Arc(agent.gridElement.position,agent.facingDirection,4,45);
            // Vector2Int[] view = GridUtility.RightFacingTriangle(agent.gridElement.position,agent.facingDirection,4,Vector2Int.one);
            foreach(Vector2Int v in view)
            {
                tilemapManager.SoundOneTile(v);
            }
        }

    }
    IEnumerator WaitAndQueuePath(Vector2Int destination)
    {
        bool pathfailed = false;
        Pathfind finder = agent.SetDestination(destination);
        while(finder.pathStatus != 1)
        {
            if(finder.pathStatus == -1)
            {
                pathfailed = true;
                break;
            }
            yield return null;
        }
        if(!pathfailed){
            while(agent.pathToDestination.Count<0)
            {
                yield return null;
            }
            int steps = Mathf.Min(gfm.playerTurnsAllowed.Value-gfm.playerTurnsTaken.Value,agent.pathToDestination.Count-1);//-1 is because first item in the queue should be current location, I think.
            Vector2Int current = agent.position;
            for(int i = 0;i<steps+1;i++)
            {
                Vector2Int next = agent.pathToDestination.Dequeue();
                if(current != next){
                    inputStack.Enqueue(next-current);
                    current = next;
                }
            }
        }
    }
}
