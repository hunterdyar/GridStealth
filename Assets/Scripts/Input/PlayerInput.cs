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
    void Awake()
    {
        agent = GetComponent<Agent>();     
    }

    // Update is called once per frame
    void Update()
    {
        if(!gfm.playerCanMove.Value){return;}
        if(activeTurn.blockPlayerMovement){return;}
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
        ///mouse Input
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
    }
}
