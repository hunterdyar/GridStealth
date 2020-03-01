using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class Agent : MonoBehaviour
{
    GridElement gridElement;
    Vector2Int position {get{return gridElement.position;}}
    public bool pushable;
    void Awake()
    {
        gridElement = GetComponent<GridElement>();
    }
    public bool Move(Vector2Int dir)
    {
        //tests
        if(dir.magnitude != 1){Debug.LogError("invalid movement for move",gameObject);}
        List<Agent> pushing = new List<Agent>();
        if(CanMoveInDir(dir, ref pushing))
        {

            transform.position = transform.position+new Vector3(dir.x,dir.y,0);
            foreach(Agent m in pushing)
            {
                m.Move(dir);                
            }
            gridElement.OnNewPosition();
            return true;
        }else
        {
            return false;//didnt move.
        }

    }
    public bool CanMoveInDir(Vector2Int dir,ref List<Agent> pushing)
    {
        TileNode next = gridElement.tilemapManager.GetTileNode(position+dir);
        if(next == null){return false;}
        // if(next.solid){return false;}
        List<GridElement> atNode = next.itemsHere;
        foreach(GridElement gein in next.itemsHere)
        {
            //can we push solid things?
            // if(gein.solid){return false;}
            if(gein.GetComponent<Agent>() != null)
            {
                Agent neigh = gein.GetComponent<Agent>();
                if(neigh.pushable){
                    if(neigh.CanMoveInDir(dir,ref pushing))
                    {
                        pushing.Add(neigh);
                    }else{
                        return false;
                    }
                }else{
                    return false;
                }
            }
        }
        return true;
    }
}
