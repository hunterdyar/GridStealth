using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
public class TilemapManager : ScriptableObject
{
    public Dictionary<Vector2Int,TileNode> level = new Dictionary<Vector2Int,TileNode>();
    public List<TileNode> allTileNodes = new List<TileNode>();
    public int brightnessScale = 10;
    static Vector2Int[] cardinalDirections = new Vector2Int[]{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    public Tilemap tilemap;
    public GameObject lightSpriteDitherPrefab;

    [Button]
    public void InitiateNodes()
    {
        level = new Dictionary<Vector2Int, TileNode>();
        allTileNodes = new List<TileNode>();
        foreach(Vector3Int p3 in tilemap.cellBounds.allPositionsWithin)
        {
            if(tilemap.HasTile(p3))
            {
                TileBase tile = tilemap.GetTile(p3);
                if(tile is LevelTile)
                {
                    TileNode node = new TileNode();
                    node.position = (Vector2Int)p3;
                    node.isSolid = ((LevelTile)tile).solid;
                    node.isOpaque = ((LevelTile)tile).opaque;
                    //Initiate things
                    LightSpriteDither dither = GameObject.Instantiate(lightSpriteDitherPrefab,tilemap.CellToWorld(p3)+new Vector3(0.5f,0.5f,0),Quaternion.identity,tilemap.transform).GetComponent<LightSpriteDither>();
                    dither.tileNode = node;
                    //
                    level.Add(node.position,node);
                    allTileNodes.Add(node);
                }
            }
        }
    }
    public void RemoveNode(Vector2Int at)
    {
        TileNode n = level[at];
        allTileNodes.Remove(n);
        level.Remove(at);
    }
    public TileNode GetTileNode(Vector2Int pos)
    {
        if(level.ContainsKey(pos))
        {
            return level[pos];
        }else
        {
            return null;
        }

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
            return GetTileNode(pos).solid;
        }else{
            return true;//empty spaces are solid
        }
    }
    public bool IsSolid(int x,int y)
    {
        return IsSolid(new Vector2Int(x,y));
    }
    public bool IsOpaque(Vector2Int pos)
    {
        if(HasLevelTile(pos))
        {
            return GetTileNode(pos).opaque;
        }else{
            return true;//empty spaces are opaque
        }
    }
    public bool IsOpaque(int x,int y)
    {
        return IsOpaque(new Vector2Int(x,y));
    }
    public TileNode[] GetNeighborsTo(TileNode gridItem)
    {
        List<TileNode> neighbors = new List<TileNode>();
        foreach(Vector2Int dir in cardinalDirections)
        {
            TileNode n = GetTileNode(gridItem.position+dir);
            if(n!=null)
            {
                neighbors.Add(n);
            }
        }
        return neighbors.ToArray();
    }

    public void UpdateBrightnessDisplay()
    {
        foreach(Vector3Int p3 in tilemap.cellBounds.allPositionsWithin)
        {
            if(HasLevelTile((Vector2Int)p3) && p3.z == 0)
            {
                //flags should be none
                tilemap.SetTileFlags(p3, TileFlags.None);
                //cool
                TileNode t = GetTileNode((Vector2Int)p3);
                // if(!t.solid){
                    float b01 = Mathf.Clamp01((float)t.brightness);///(float)brightnessScale
                    // Debug.Log("setting tile "+p3+" to "+b01);
                    Color brightColor = Color.white;//
                    //tilemap.SetColor(p3,Color.Lerp(Color.black,brightColor,b01));
                // }
            }
        }
    }


    //See superCover
    public bool LineOfSight(TileNode a,TileNode b)
    {
        return LineOfSight(a.position,b.position);
    }
    public bool LineOfSight(Vector2Int a,Vector2Int b)
    {
        int i;               // loop counter 
        int ystep, xstep;    // the step on y and x axis 
        int error;           // the error accumulated during the increment 
        int errorprev;       // *vision the previous value of the error variable 
        int y = a.y;
        int x = a.x;  // the line points 
        int ddy, ddx;        // compulsory variables: the double values of dy and dx 
        int dx = b.x - a.x; 
        int dy = b.y - a.y; 
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
                    if(IsOpaque(x,y))
                    {
                        return false;
                    }
                    //
                    if (error + errorprev < ddx && !IsOpaque(x,y-ystep))
                    {
                        // Debug.Log("case a");
                        // GetItem(new Vector2Int(x,y-ystep)).GetComponent<SpriteRenderer>().color = Color.red;
                    }  // bottom square also 
                    else if (error + errorprev > ddx && !IsOpaque(x-xstep,y))
                    {  // left square also 
                        // Debug.Log("case b");
                        // GetItem(new Vector2Int(x-xstep,y)).GetComponent<SpriteRenderer>().color = Color.green;

                    }else if(!IsOpaque(x,y-ystep) || !IsOpaque(x-xstep,y)){
                        // Debug.Log("case c");
                        // GetItem(new Vector2Int(x-xstep,y)).GetComponent<SpriteRenderer>().color = Color.green;

                    }else{
                        return false;
                    }                  
                }
                else
                {
                    if(IsOpaque(x,y))
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
                if(IsOpaque(x,y)){return false;}

                if (error + errorprev < ddy && !IsOpaque(x-xstep,y))
                {
                    //case a
                }else if (error + errorprev > ddy && !IsOpaque(x,y-ystep))
                {
                    //case b
                }else if(!IsOpaque(x-xstep,y) || !IsOpaque(x,y-ystep))
                {
                    //case c
                }else{
                    return false;
                } 
            }
            else
            {
                if(IsOpaque(x,y))
                {
                    return false;
                }
            }
            // GetItem(new Vector2Int(x,y)).GetComponent<SpriteRenderer>().color = Color.green; 
            errorprev = error; 
            } 
        }
        //assert
        return ((y == b.y) && (x == b.x));
    }
}
