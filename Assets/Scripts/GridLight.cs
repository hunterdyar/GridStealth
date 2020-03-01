using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[RequireComponent(typeof(GridElement))]
public class GridLight : MonoBehaviour
{
    Vector2Int position {get{return gridElement.position;}}
    GridElement gridElement;
    public int lightIntensity;
    public int lightRange;
    [Button("Assign gridElement")]
    void Awake()
    {
        gridElement = GetComponent<GridElement>();
        gridElement.OnNewPositionAction += Illuminate;
    }
    [Button("Illuminate")]
    void Illuminate()
    {
        //remove self from all levelNodes.
        foreach(TileNode t in gridElement.tilemapManager.allTileNodes)
        {
            if(t.lightsOnMe.Contains(this)){
                t.lightsOnMe.Remove(this);
                t.SetBrightness();
            }
        }

        Vector2Int[] c = GridUtility.Circle(position,lightRange);
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

    public int BrightnessForTile(Vector2Int tilePos)
    {
        return (int)Mathf.Max(0,((lightIntensity - GridUtility.ManhattanDistance(position,tilePos))/(float)lightIntensity)*gridElement.tilemapManager.brightnessScale);
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
