using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : Agent
{
    public GameFlowManager gfm;
    protected override void MoveEnded()
    {
        base.MoveEnded();
        gfm.PlayerTookTurn();        
    }
}
