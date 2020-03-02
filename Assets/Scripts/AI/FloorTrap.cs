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
        List<GridElement> itemsHereNow = new List<GridElement>(gridElement.tileNode.itemsHere);
        //clone the list becuase we may modify it when we move things that are in the lsit
        foreach(GridElement ge in itemsHereNow)
        {
            if(ge.GetComponent<Agent>() != null){
                Debug.Log("floor trap on "+ge.gameObject.name);
                ge.GetComponent<Agent>().Move(GridUtility.DirToV2(pushDirection));
            }
        }
        return info;
    }
}
