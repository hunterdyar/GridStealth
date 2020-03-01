using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLight : MonoBehaviour
{
    public TilemapManager tilemapManager;
    public int brightness;
    public Vector2Int position{get{return GridPosition();}}
    
    
    public Vector2Int GridPosition()
    {
        return tilemapManager.WorldToCell(transform.position);
    }
    [ContextMenu("Illuminate")]
    void Illuminate()
    {
        Debug.Log("illuminate");
        Vector2Int[] c = GridUtility.Circle(position,brightness);
        Debug.Log("need up update "+c.Length+" tiles");
        foreach(Vector2Int p in c)
        {
            LevelTile gi = tilemapManager.GetLevelTile(p);
            if(gi!=null)
            {
                Debug.Log("updating brightness for "+gi.position);

                if(tilemapManager.LineOfSight(tilemapManager.GetLevelTile(position),gi))
                {
                    gi.brightness = brightness - GridUtility.ManhattanDistance(position,gi.position);
                }
            }
        }
        tilemapManager.UpdateBrightnessDisplay();
    }


    void SetBrightnessOfNeighbors(LevelTile starting, int depth, int maxDepth,ref List<Vector2Int> checkedN)
    {
        checkedN.Add(starting.position);
        List<LevelTile> ns = new List<LevelTile>(tilemapManager.GetNeighborsTo(starting));
        foreach(LevelTile n in ns)
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
