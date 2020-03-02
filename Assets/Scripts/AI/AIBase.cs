using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(GridElement))]
public class AIBase : MonoBehaviour, IComparable<AIBase>
{
    public GameFlowManager gameFlowManager;

    public GridElement gridElement;

    void Awake()
    {
        gridElement = GetComponent<GridElement>();
    }
    public void OnEnable()
    {
        gridElement = GetComponent<GridElement>();
        gameFlowManager.RegisterAI((AIBase)this);
    }
    public void OnDisable()
    {
        gameFlowManager.DeRegisterAI((AIBase)this);
    }
    public virtual TurnInfo TakeTurn()
    {
        TurnInfo info = new TurnInfo(this);
        info.turnInMotion = false;
        info.blockPlayerMovement = false;
        return info;
    }

    public int CompareTo(AIBase comparePart)
    {
        // A null value means that this object is greater.
        if (comparePart == null)
        {
            return 1;
        }   
        else
        {
            //x L to R
            if(comparePart.gridElement.position.x < gridElement.position.x){return 1;}
            else if(comparePart.gridElement.position.x > gridElement.position.x){return -1;}
            else{

                //y for x tie, Top to Bottom
                if(comparePart.gridElement.position.y > gridElement.position.x){return 1;}
                else if(comparePart.gridElement.position.y < gridElement.position.x){return -1;}
                else
                {
                    return 0;
                }
            }
        }
    }
}
