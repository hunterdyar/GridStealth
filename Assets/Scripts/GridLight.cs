using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class GridLight : GridElement
{
    
    
    [Button("Illuminate")]
    void Illuminate()
    {
        //remove self from all levelNodes.
        foreach(TileNode t in tilemapManager.allTileNodes)
        {
            if(t.lightsOnMe.Contains(this)){
                t.lightsOnMe.Remove(this);
                t.SetBrightness();
            }
        }

        Vector2Int[] c = GridUtility.Circle(position,brightness);
        foreach(Vector2Int p in c)
        {
            TileNode tn = tilemapManager.GetTileNode(p);
            if(tn!=null)
            {
                if(tilemapManager.LineOfSight(tn.position,position)){
                    if(!tn.lightsOnMe.Contains(this)){
                        tn.lightsOnMe.Add(this);
                        tn.SetBrightness();
                    }
                }
            }
        }
        tilemapManager.UpdateBrightnessDisplay();
    }

    public override void OnNewPosition(){
        base.OnNewPosition();
        Illuminate();
    }
    public int BrightnessForTile(Vector2Int tilePos)
    {
        return brightness - GridUtility.ManhattanDistance(position,tilePos);
    }
    void SetBrightnessOfNeighbors(TileNode starting, int depth, int maxDepth,ref List<Vector2Int> checkedN)
    {
        checkedN.Add(starting.position);
        List<TileNode> ns = new List<TileNode>(tilemapManager.GetNeighborsTo(starting));
        foreach(TileNode n in ns)
        {
            if(depth <= maxDepth)
            {
                if(!checkedN.Contains(n.position))
                {
                    // if(holdingGrid.LineOfSight(this,n))
                    // {
                        n.brightness = brightness - GridUtility.ManhattanDistance(position,n.position);
                        checkedN.Add(n.position);
                        //Recursion!
                        SetBrightnessOfNeighbors(n,depth+1,maxDepth,ref checkedN);
                    // }else{Debug.Log("no line of sight,"+n.position);}
                }else{Debug.Log("already checked, "+n.position);}
            }else{Debug.Log("max depth past, "+depth+", "+n.position);}
        }
    }
}
