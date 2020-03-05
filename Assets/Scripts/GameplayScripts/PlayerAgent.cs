using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
public class PlayerAgent : Agent
{
    public GameFlowManager gfm;
    public SharedVector2Int sharedPosition;
    protected void Start()
    {
        SharedAgent playerAgent = new SharedAgent();
        playerAgent.Value = this;
        GlobalVariables.Instance.SetVariable("playerAgent", playerAgent);
        sharedPosition.Value = position;
    }
    protected override void MoveEnded(TurnInfo turn)
    {
        sharedPosition.Value = position;
        GlobalVariables.Instance.SetVariable("playerPosition", sharedPosition);
        base.MoveEnded(turn);
        if(turn.useUpTurn){
            gfm.PlayerTookTurn();
        }        
    }
}
