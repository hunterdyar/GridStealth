using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public Dictionary<Vector2Int, GridItem> grid;
    public static Vector2Int[] cardinalDirections = new Vector2Int[]{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    public GameObject tempSquare;

    void Start()
    {
        // Debug.Log("Line of sight? "+LineOfSight(grid[new Vector2Int(0,9)],grid[new Vector2Int(9,0)]));
        
    }
    public GridItem[] GetAllItems()
    {
        List<GridItem> gridItems = new List<GridItem>();
        foreach(GridItem g in grid.Values)
        {
            gridItems.Add(g);
        }
        return gridItems.ToArray();
    }
    public GridItem GetItem(Vector2Int pos)
    {
        if(grid.ContainsKey(pos))
        {
            return grid[pos];
        }else{
            return null;
        }
    }
    public bool HasItem(Vector2Int pos)
    {
        return grid.ContainsKey(pos);
    }
    public bool IsSolid(Vector2Int pos)
    {
        if(grid.ContainsKey(pos))
        {
            return grid[pos].solid;
        }else{
            return true;
        }
    }
    public bool IsSolid(int x,int y)
    {
        return IsSolid(new Vector2Int(x,y));
    }
    public void AddGridItem(Vector2Int pos, GridItem item)
    {
        if(grid == null)
        {
            InitiateGrid();
        }
        if(!grid.ContainsKey(pos)){
            grid.Add(pos,item);
        }
        //else return false;
    }
    void InitiateGrid()
    {
        grid = new Dictionary<Vector2Int, GridItem>();
    }
    public static int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x)+Mathf.Abs(a.y-b.y);
    }
    public GridItem[] GetNeighborsTo(GridItem gridItem)
    {
        List<GridItem> neighbors = new List<GridItem>();
        foreach(Vector2Int dir in cardinalDirections)
        {
            GridItem n = GetItem(gridItem.position+dir);
            if(n!=null)
            {
                neighbors.Add(n);
            }
        }
        return neighbors.ToArray();
    }
    [ContextMenu("Generate Grid")]
    public void GenerateQuickTable()
    {
        GenerateTable(10,10);
    }
    public void GenerateTable(int width,int height)
    {
        for(int x = 0;x<width;x++)
        {
            for(int y = 0;y<width;y++)
            {
                GameObject.Instantiate(tempSquare,new Vector3(x,y,0),Quaternion.identity);
            }
        }
    }

    //See superCover below
    public bool LineOfSight(GridItem a,GridItem b)
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
                        GetItem(new Vector2Int(x,y-ystep)).GetComponent<SpriteRenderer>().color = Color.red;
                    }  // bottom square also 
                    else if (error + errorprev > ddx && !IsSolid(x-xstep,y))
                    {  // left square also 
                        // Debug.Log("case b");
                        GetItem(new Vector2Int(x-xstep,y)).GetComponent<SpriteRenderer>().color = Color.green;

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
                GetItem(new Vector2Int(x,y)).GetComponent<SpriteRenderer>().color = Color.blue; //debugging

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
            GetItem(new Vector2Int(x,y)).GetComponent<SpriteRenderer>().color = Color.green; 
            errorprev = error; 
            } 
        }
        //assert
        return ((y == b.position.y) && (x == b.position.x));
    }
    //http://eugen.dedu.free.fr/projects/bresenham/
    public static Vector2Int[] SuperCover(Vector2Int a,Vector2Int b)
    {

        List<Vector2Int> blocks = new List<Vector2Int>();
        int i;               // loop counter 
        int ystep, xstep;    // the step on y and x axis 
        int error;           // the error accumulated during the increment 
        int errorprev;       // *vision the previous value of the error variable 
        int y = a.y;
        int x = a.x;  // the line points 
        int ddy, ddx;        // compulsory variables: the double values of dy and dx 
        int dx = b.x - a.x; 
        int dy = b.y - a.y; 
        blocks.Add(a);  // first point 
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
        if (ddx >= ddy){  // first octant (0 <= slope <= 1) 
            // compulsory initialization (even for errorprev, needed when dx==dy) 
            errorprev = error = dx;  // start in the middle of the square 
            for (i=0 ; i < dx ; i++){  // do not use the first point (already done) 
            x += xstep; 
            error += ddy; 
            if (error > ddx){  // increment y if AFTER the middle ( > ) 
                y += ystep; 
                error -= ddx; 
                // three cases (octant == right->right-top for directions below): 
                if (error + errorprev < ddx)  // bottom square also 
                blocks.Add(new Vector2Int(x,y-ystep)); 
                else if (error + errorprev > ddx)  // left square also 
                blocks.Add(new Vector2Int(x-xstep,y)); 
                else{  // corner: bottom and left squares also 
                blocks.Add(new Vector2Int(x,y-ystep)); 
                blocks.Add(new Vector2Int(x-xstep,y)); 
                } 
            } 
            blocks.Add(new Vector2Int (x, y)); 
            errorprev = error; 
            } 
        }else{  // the same as above 
            errorprev = error = dy; 
            for (i=0 ; i < dy ; i++){ 
            y += ystep; 
            error += ddx; 
            if (error > ddy){ 
                x += xstep; 
                error -= ddy; 
                if (error + errorprev < ddy) 
                blocks.Add(new Vector2Int (x-xstep,y)); 
                else if (error + errorprev > ddy) 
                blocks.Add(new Vector2Int (x,y-ystep)); 
                else{ 
                blocks.Add(new Vector2Int (x-xstep,y)); 
                blocks.Add(new Vector2Int (x,y-ystep)); 
                } 
            } 
            blocks.Add(new Vector2Int (x, y)); 
            errorprev = error; 
            } 
        } 
    // assert ((y == y2) && (x == x2));  // the last point (y2,x2) has to be the same with the last point of the algorithm 
    return blocks.ToArray();
    }

    public static Vector2Int[] Bresenham(Vector2Int a, Vector2Int b)
    {
        List<Vector2Int> blocks = new List<Vector2Int>();
        int dx = (int)Mathf.Abs(a.x-b.x);//delta x
        int dy = (int)Mathf.Abs(a.y-b.y);//delta y
        //dx/dy is our slope.
        //initial x and y.
        int x = a.x;
        int y = a.y;
        //total march steps to take
        int n = 1+dx+dy;
        //
        int x_inc = (b.x > a.x) ? 1 : -1;
        int y_inc = (b.y > a.y) ? 1 : -1;
        int error = dx - dy;
        //Multiply by 2 so we go from the center of the blocks out, basically.
        dx *= 2;
        dy *= 2;

        //
        for (; n > 0; --n)//i got this code from somewhere and i've fucken never seen this syntax before lol. n already exists, and we n-- while its greater than 0.
        {
            blocks.Add(new Vector2Int(x,y));
            //
            if(error == 0){
                //vertical block for now?
                y += y_inc;
                error += dx;
                //uh?
            }else if (error > 0)
            {
                x += x_inc;
                error -= dy;
            }
            else
            {
                y += y_inc;
                error += dx;
            }
        }
        return blocks.ToArray();
    }
    public static Vector2Int[] BresenCircle(Vector2Int a, int radius)
    {
            // void plotCircle(int xm, int ym, int r)
        int r = radius;
        int xm = a.x;
        int ym = a.y;
        List<Vector2Int> c = new List<Vector2Int>();
        int x = -r, y = 0, err = 2-2*r; /* II. Quadrant */ 
        do {
            c.Add(new Vector2Int(xm-x, ym+y)); /*   I. Quadrant */
            c.Add(new Vector2Int(xm-y, ym-x)); /*  II. Quadrant */
            c.Add(new Vector2Int(xm+x, ym-y)); /* III. Quadrant */
            c.Add(new Vector2Int(xm+y, ym+x)); /*  IV. Quadrant */
            r = err;
            if (r <= y) err += ++y*2+1;           /* e_xy+e_y < 0 */
            if (r > x || err > y) err += ++x*2+1; /* e_xy+e_x > 0 or no 2nd y-step */
        } while (x < 0);
        
        return c.ToArray();
    }
}