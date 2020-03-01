using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLight : GridItem
{
    public List<Vector2Int> c;
    public int myBrightness;
    [ContextMenu("Illuminate")]
    void Illuminate()
    {
        brightness = myBrightness;
        c = new List<Vector2Int>();
        SetBrightnessOfNeighbors(this,0,(int)myBrightness,ref c);
        // Vector2Int[] c = GridManager.BresenCircle(position,myBrightness);
                // holdingGrid = GameObject.FindObjectOfType<GridManager>();

        // foreach(Vector2Int p in c)
        // {
            
        //     GridItem gi = holdingGrid.GetItem(p);
        //     if(gi!=null)
        //     {
        //         gi.brightness = myBrightness;
        //     }
        // }
    }


    void SetBrightnessOfNeighbors(GridItem starting, int depth, int maxDepth,ref List<Vector2Int> checkedN)
    {
        checkedN.Add(starting.position);
        holdingGrid = GameObject.FindObjectOfType<GridManager>();
        List<GridItem> ns = new List<GridItem>(holdingGrid.GetNeighborsTo(starting));
        foreach(GridItem n in ns)
        {
            if(depth <= maxDepth)
            {
                if(!checkedN.Contains(n.position))
                {
                    if(holdingGrid.LineOfSight(this,n))
                    {
                        n.brightness = myBrightness - GridManager.ManhattanDistance(position,n.position);
                        checkedN.Add(n.position);
                        //Recursion!
                        SetBrightnessOfNeighbors(n,depth+1,maxDepth,ref checkedN);
                    }else{Debug.Log("no line of sight,"+n.position);}
                }else{Debug.Log("already checked, "+n.position);}
            }else{Debug.Log("max depth past, "+depth+", "+n.position);}
        }
    }
}
