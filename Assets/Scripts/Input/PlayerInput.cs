using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class PlayerInput : MonoBehaviour
{
    Agent agent;
    void Awake()
    {
        agent = GetComponent<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            agent.Move(Vector2Int.right);
        }else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            agent.Move(Vector2Int.left);
        }else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            agent.Move(Vector2Int.up);
        }else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            agent.Move(Vector2Int.down);
        }
    }
}
