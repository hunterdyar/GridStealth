using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrap : AIBase
{
    public GridDirection pushDirection;
    public override TurnInfo TakeTurn()
    {
        TurnInfo info = new TurnInfo(this);
        //check any objects that are here, and if they have playerInput (better player check?)
        //push em.
        foreach(GridElement ge in gridElement.tileNode.itemsHere)
        {
            if(ge.GetComponent<Agent>() != null){
                ge.GetComponent<Agent>().Move(GridUtility.DirToV2(pushDirection));
            }
        }
        return info;
    }
}
