using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class Agent : MonoBehaviour
{
    GridElement gridElement;
    public Vector2Int position {get{return gridElement.position;}}
    public bool pushable;
    void Awake()
    {
        gridElement = GetComponent<GridElement>();
        if(gridElement.solid && pushable)
        {
            Debug.LogWarning("Agent is pushable but gridElement is set to solid. overriding gridElement solid to false");
            gridElement.solid = false;
        }
    }
    public TurnInfo Move(Vector2Int dir)
    {
        TurnInfo info = new TurnInfo();
        return DoMove(dir,info);
    }
    protected virtual void MoveEnded()
    {
        gridElement.OnNewPosition();
    }

    public TurnInfo DoMove(Vector2Int dir, TurnInfo info)
    {
        //tests
        if(dir.magnitude != 1){Debug.LogError("invalid movement for move",gameObject);}
        List<Agent> pushing = new List<Agent>();
        if(CanMoveInDir(dir, ref pushing))
        {
           
            info.blockPlayerMovement = true;
            info.endOfMoveAction += MoveEnded;
            StartCoroutine(Lerp(transform.position,transform.position+new Vector3(dir.x,dir.y,0),0.33f, info));
            foreach(Agent m in pushing)
            {
                m.Move(dir);                
            }
            gridElement.OnNewPosition();
        }
        return info;
    }
    public IEnumerator Lerp(Vector3 start,Vector3 end, float timeToMove, TurnInfo info)
    {
        float t = 0;
        while(t<=timeToMove)
        {
            transform.position = Vector3.Lerp(start,end,t);
            t = t+Time.deltaTime/timeToMove;
            yield return null;
        }
        transform.position = end;
        info.Finish();
    }
    public bool CanMoveInDir(Vector2Int dir,ref List<Agent> pushing)
    {
        TileNode next = gridElement.tilemapManager.GetTileNode(position+dir);
        if(next == null){return false;}
        if(next.solid){return false;}
        List<GridElement> atNode = next.itemsHere;
        foreach(GridElement gein in next.itemsHere)
        {
            //can we push solid things?
            if(gein.solid){return false;}
            if(gein.GetComponent<Agent>() != null)
            {
                Agent neigh = gein.GetComponent<Agent>();
                if(neigh.pushable && neigh != this){
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
