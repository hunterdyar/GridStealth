using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfo
{
    public bool turnTaken = true;
    public bool turnInMotion = false;
    public bool blockPlayerMovement = false;
    public bool blockAIMovement = false;
    public AIBase ai;
    public GridElement gridElement;
    public Vector2Int initialLocation;
    public TurnInfo(){
        turnInMotion = false;
        ai = null;
        gridElement = null;
    }
    public TurnInfo(AIBase aibase)
    {
        turnInMotion = false;
        ai = aibase;
        gridElement = aibase.gridElement;
        initialLocation = gridElement.position;
    }
    public void Finish()
    {
        turnInMotion = false;
        blockPlayerMovement = false;
        blockAIMovement = false;
    }
}
