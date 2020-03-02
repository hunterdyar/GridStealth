using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class PlayerInput : MonoBehaviour
{
    Agent agent;
    public GameFlowManager gfm;
    void Awake()
    {
        agent = GetComponent<Agent>();     
    }

    // Update is called once per frame
    void Update()
    {
        if(!gfm.playerCanMove){return;}
        TurnInfo moved = new TurnInfo();//this will get replaced if we move
        moved.turnTaken = false;//never true by default
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            moved = agent.Move(Vector2Int.right);
        }else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moved = agent.Move(Vector2Int.left);
        }else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            moved = agent.Move(Vector2Int.up);
        }else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            moved = agent.Move(Vector2Int.down);
        }
        //
    }
}
