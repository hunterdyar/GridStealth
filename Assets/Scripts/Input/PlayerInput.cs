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
        //temp init code for gfm
        gfm.Init();
        GameFlowManager.instance = gfm;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gfm.playerCanMove){return;}
        bool moved = false;
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
        if(moved){gfm.PlayerTookTurn();}
    }
}
