using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class PlayerInput : MonoBehaviour
{
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
        ///
        if(!activeTurn.blockPlayerMovement && inputStack.Count > 0)
        {
            activeTurn = agent.Move(inputStack.Dequeue());
            activeTurn.blockPlayerMovement = true;
        }
    }
}
