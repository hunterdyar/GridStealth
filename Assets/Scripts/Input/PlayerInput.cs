using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Agent))]
public class PlayerInput : MonoBehaviour
{
    public TilemapManager tilemapManager;
    Agent agent;
    public GameFlowManager gfm;
    TurnInfo activeTurn = new TurnInfo();
    public Queue<Vector2Int> inputStack = new Queue<Vector2Int>();
    Pathfind pathfind;
    Coroutine ambientPathfinding;
    void Awake()
    {
        pathfind = new Pathfind();
        pathfind.tilemapManager = tilemapManager;
        agent = GetComponent<Agent>();     
    }
    void OnEnable()
    {
        gfm.PlayerInNewLocationAction += CachePathfind;
    }
    void OnDisable()
    {
        gfm.PlayerInNewLocationAction -= CachePathfind;
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

            StartCoroutine(WaitForPathThenMove(clickNode));

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

        // if(Input.GetKeyDown(KeyCode.A)){
        //     Vector2Int[] view = GridUtility.Arc(agent.position,Vector2Int.one,6);
        //     foreach(Vector2Int v in view)
        //     {
        //         Debug.Log(v);
        //         tilemapManager.Sound(v,1);
        //     }
        // }
    }
    IEnumerator WaitForPathThenMove(TileNode clickNode)
    {
        pathfind.Search(agent.gridElement.tileNode,clickNode,this);

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
                if(pathfind.distances[clickNode] <= gfm.playerTurnsAllowed.Value - gfm.playerTurnsTaken.Value){//too far away to move during this turn
                    for(int i = 1;i<pathfind.path.Count;i++)
                    {
                        inputStack.Enqueue(pathfind.path[i].position-pathfind.path[i-1].position);
                    }
                }
            }
        }
    }
    
    void CachePathfind()
    {
        if(pathfind.running)
        {
            StopCoroutine(ambientPathfinding);
        }
        ambientPathfinding = StartCoroutine(pathfind.FindAllPaths(agent.gridElement.tileNode,10));
    }
}
