using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapManager : MonoBehaviour
{
    public int brightnessScale = 10;
    static Vector2Int[] cardinalDirections = new Vector2Int[]{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    public Vector3Int testingToRed;
    public Tilemap tilemap;
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    [ContextMenu("Testing To Red To Red")]
    void TestingToRed()
    {
        tilemap.SetColor(testingToRed,Color.red);
    }
    public Vector2Int WorldToCell(Vector3 worldPos){
        return (Vector2Int)tilemap.WorldToCell(worldPos);
    }
    public LevelTile GetLevelTile(Vector2Int pos)
    {
        TileBase t = tilemap.GetTile((Vector3Int)pos);
        ///
        if(t == null)
        {
            //no any tile
            return null;
        }
        else if(t is LevelTile)
        {
            //tile is a levelTile.
            return (LevelTile)t;
        }else
        {
            //tile but not levelTile.
            return null;
        }
    }
    public bool HasLevelTile(Vector2Int pos)
    {
        
        if(tilemap.HasTile((Vector3Int)pos))
        {
            TileBase t = tilemap.GetTile((Vector3Int)pos);
            return (t is LevelTile);
        }else
        {
            return false;
        }
    }
    public bool IsSolid(Vector2Int pos)
    {
        if(HasLevelTile(pos))
        {
            return GetLevelTile(pos).solid;
        }else{
            return true;//empty spaces are solid
        }
    }
    public bool IsSolid(int x,int y)
    {
        return IsSolid(new Vector2Int(x,y));
    }
    public LevelTile[] GetNeighborsTo(LevelTile gridItem)
    {
        List<LevelTile> neighbors = new List<LevelTile>();
        foreach(Vector2Int dir in cardinalDirections)
        {
            LevelTile n = GetLevelTile(gridItem.position+dir);
            if(n!=null)
            {
                neighbors.Add(n);
            }
        }
        return neighbors.ToArray();
    }

    public void UpdateBrightnessDisplay()
    {
        Debug.Log("updating brightness display");
        foreach(Vector3Int p3 in tilemap.cellBounds.allPositionsWithin)
        {
            if(HasLevelTile((Vector2Int)p3) && p3.z == 0)
            {
                //flags should be none
                tilemap.SetTileFlags(p3, TileFlags.None);
                //cool
                LevelTile t = GetLevelTile((Vector2Int)p3);
                float b01 = Mathf.Clamp01((float)t.brightness/(float)brightnessScale);//brightness mapped 0-1;
                // Debug.Log("setting tile "+p3+" to "+b01);
                tilemap.SetColor(p3,new Color(b01,b01,b01,1));
            }
        }
    }


    //See superCover
    public bool LineOfSight(LevelTile a,LevelTile b)
    {
        int i;               // loop counter 
        int ystep, xstep;    // the step on y and x axis 
        int error;           // the error accumulated during the increment 
        int errorprev;       // *vision the previous value of the error variable 
        int y = a.position.y;
        int x = a.position.x;  // the line points 
        int ddy, ddx;        // compulsory variables: the double values of dy and dx 
        int dx = b.position.x - a.position.x; 
        int dy = b.position.y - a.position.y; 
        // NB the last point can't be here, because of its previous point (which has to be verified) 
        if (dy < 0){ 
            ystep = -1; 
            dy = -dy; 
        }else 
            ystep = 1; 
        if (dx < 0){ 
            xstep = -1; 
            dx = -dx; 
        }else 
            xstep = 1; 
        ddy = 2 * dy;  // work with double values for full precision 
        ddx = 2 * dx; 
        //
        if (ddx >= ddy)
        {  // first octant (0 <= slope <= 1) 
            // compulsory initialization (even for errorprev, needed when dx==dy) 
            errorprev = error = dx;  // start in the middle of the square 
            for (i=0 ; i < dx ; i++)
            {  // do not use the first point (already done) 
                x += xstep; 
                error += ddy; 
                if (error > ddx)
                {  // increment y if AFTER the middle ( > ) 
                    y += ystep; 
                    error -= ddx; 
                    // three cases (octant == right->right-top for directions below): 
                    if(IsSolid(x,y))
                    {
                        return false;
                    }
                    //
                    if (error + errorprev < ddx && !IsSolid(x,y-ystep))
                    {
                        // Debug.Log("case a");
                        // GetItem(new Vector2Int(x,y-ystep)).GetComponent<SpriteRenderer>().color = Color.red;
                    }  // bottom square also 
                    else if (error + errorprev > ddx && !IsSolid(x-xstep,y))
                    {  // left square also 
                        // Debug.Log("case b");
                        // GetItem(new Vector2Int(x-xstep,y)).GetComponent<SpriteRenderer>().color = Color.green;

                    }else if(!IsSolid(x,y-ystep) || !IsSolid(x-xstep,y)){
                        // Debug.Log("case c");
                        // GetItem(new Vector2Int(x-xstep,y)).GetComponent<SpriteRenderer>().color = Color.green;

                    }else{
                        return false;
                    }                  
                }
                else
                {
                    if(IsSolid(x,y))
                    {
                        return false;
                    }
                }
                // GetItem(new Vector2Int(x,y)).GetComponent<SpriteRenderer>().color = Color.blue; //debugging

                errorprev = error; 
            } 
        }else
        {  // the same as above 
            errorprev = error = dy; 
            for (i=0 ; i < dy ; i++)
            { 
            y += ystep; 
            error += ddx; 
            if (error > ddy)
            { 
                x += xstep; 
                error -= ddy; 
                if(IsSolid(x,y)){return false;}

                if (error + errorprev < ddy && !IsSolid(x-xstep,y))
                {
                    //case a
                }else if (error + errorprev > ddy && !IsSolid(x,y-ystep))
                {
                    //case b
                }else if(!IsSolid(x-xstep,y) || !IsSolid(x,y-ystep))
                {
                    //case c
                }else{
                    return false;
                } 
            }
            else
            {
                if(IsSolid(x,y))
                {
                    return false;
                }
            }
            // GetItem(new Vector2Int(x,y)).GetComponent<SpriteRenderer>().color = Color.green; 
            errorprev = error; 
            } 
        }
        //assert
        return ((y == b.position.y) && (x == b.position.x));
    }
}
