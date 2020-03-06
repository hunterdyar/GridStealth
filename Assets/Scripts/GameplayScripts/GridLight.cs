using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum GridLightType{
    circle,
    arc
}

[RequireComponent(typeof(GridElement))]
public class GridLight : MonoBehaviour
{
    Vector2Int position => gridElement.position;
    GridElement gridElement;
    Agent agent;
    public AnimationCurve lightFalloffCurve;
    public GameFlowManager gfm;
    public GridLightType gridLightType;
    public int lightRange;
    [Button("Assign gridElement")]
    void Awake()
    {
        gridElement = GetComponent<GridElement>();
        agent = GetComponent<Agent>();
        gridElement.OnNewPositionAction += Illuminate;
    }
    void OnEnable()
    {
        gfm.PostGridElementsUpdatedAction += Illuminate;
    }
    void OnDisable()
    {
        gfm.PostGridElementsUpdatedAction -= Illuminate;
    }
    [Button("Illuminate")]
    void Illuminate()
    {
        if(!enabled){return;}
        //remove self from all levelNodes.
        foreach(TileNode t in gridElement.tilemapManager.allTileNodes)
        {
            if(t.lightsOnMe.Contains(this)){
                t.lightsOnMe.Remove(this);
                t.SetBrightness();
            }
        }
        Vector2Int[] c = new Vector2Int[0];
        if(gridLightType == GridLightType.circle){
            c = GridUtility.Circle(position,lightRange);
        }else if(gridLightType == GridLightType.arc)
        {
            c = GridUtility.Arc(position,agent.facingDirection,lightRange,45);
        }
        foreach(Vector2Int p in c)
        {
            TileNode tn = gridElement.tilemapManager.GetTileNode(p);
            if(tn!=null)
            {
                if(gridElement.tilemapManager.LineOfSight(tn.position,position)){
                    if(!tn.lightsOnMe.Contains(this)){
                        tn.lightsOnMe.Add(this);
                        tn.SetBrightness();
                    }
                }
            }
        }
        gridElement.tilemapManager.UpdateBrightnessDisplay();
    }

    public float BrightnessForTile(Vector2Int tilePos)
    {
        return (lightFalloffCurve.Evaluate((lightRange - GridUtility.ManhattanDistance(position,tilePos))/(float)lightRange));
    }
    void SetBrightnessOfNeighbors(TileNode starting, int depth, int maxDepth,ref List<Vector2Int> checkedN)
    {
        checkedN.Add(starting.position);
        List<TileNode> ns = new List<TileNode>(gridElement.tilemapManager.GetNeighborsTo(starting));
        foreach(TileNode n in ns)
        {
            if(depth <= maxDepth)
            {
                if(!checkedN.Contains(n.position))
                {
                    // if(holdingGrid.LineOfSight(this,n))
                    // {
                        n.brightness = gridElement.brightness - GridUtility.ManhattanDistance(position,n.position);
                        checkedN.Add(n.position);
                        //Recursion!
                        SetBrightnessOfNeighbors(n,depth+1,maxDepth,ref checkedN);
                    // }else{Debug.Log("no line of sight,"+n.position);}
                }else{Debug.Log("already checked, "+n.position);}
            }else{Debug.Log("max depth past, "+depth+", "+n.position);}
        }
    }
}
