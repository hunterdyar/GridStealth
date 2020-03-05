using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridDirection
{
    up,
    left,
    right,
    down,
    none
}
public class GridUtility 
{
    public static int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x)+Mathf.Abs(a.y-b.y);
    }
    public static Vector2Int DirToV2(GridDirection direction)
    {
        if(direction == GridDirection.up)
        { return Vector2Int.up;}
        else if(direction == GridDirection.right)
        { return Vector2Int.right;}
        else if(direction == GridDirection.left)
        { return Vector2Int.left;}
        else if(direction == GridDirection.down){
            return Vector2Int.down;
        }else{
            return Vector2Int.zero;
        }
    }
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
    public static Vector2Int[] BresenCircumfrence(Vector2Int a, int radius)
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
    public static Vector2Int[] Circle(Vector2Int center, int radius)
    {
        
        
        int x = center.x;
        int y = center.y;

        List<Vector2Int> c = new List<Vector2Int>();
        int r = radius;    
        int r2 = r * r;
        int area = r2 << 2;
        int rr = r << 1;

        for (int i = 0; i < area; i++)
        {
            int tx = (i % rr) - r;
            int ty = (i / rr) - r;

            if (tx * tx + ty * ty <= r2)
            {
                c.Add(new Vector2Int(x+tx,y+ty));
            }

        }
        return c.ToArray();
    }
    public static Vector2Int[] CircleManhattan(Vector2Int center, int radius)
    {
        
        
        int x = center.x;
        int y = center.y;

        List<Vector2Int> c = new List<Vector2Int>();
        int r = radius;    
        int r2 = r * r;
        int area = r2 << 2;
        int rr = r << 1;

        for (int i = 0; i < area; i++)
        {
            int tx = (i % rr) - r;
            int ty = (i / rr) - r;

            if (tx * tx + ty * ty <= r2)
            {
                if(ManhattanDistance(center,new Vector2Int(x+tx,y+ty))<=radius){
                    c.Add(new Vector2Int(x+tx,y+ty));
                }
            }

        }
        return c.ToArray();
    }
    public static int Quadrant(Vector2Int a)
    {
        //1-4, counter clockwise from (1,1) quadrant.
        if (a == Vector2Int.zero){return 0;}
        if(a.x>0)
        {
            if(a.y>0)
            {
                return 1;
            }else
            {
                return 4;
            }
        }else{
            if(a.y>0)
            {
                return 2;
            }else{
                return 3;
            }
        }  
    }
    public static Vector2Int[] Arc(Vector2Int center, Vector2Int dir, int radius,float fieldOfViewAngle){
        List<Vector2Int> arci = new List<Vector2Int>();

        // fieldOfViewAngle = fieldOfViewAngle/2;
        Vector2Int c = (center+dir*radius);
        //arctangent vs atan2? https://en.wikipedia.org/wiki/Atan2
        float atan = Mathf.Atan2(dir.y,dir.x);
        float startTan = (atan+fieldOfViewAngle*Mathf.Deg2Rad);
        float endTan = (atan-fieldOfViewAngle*Mathf.Deg2Rad);
        
        Debug.Log("arctan: "+atan);
        Debug.Log("starting angle = "+startTan);
        Debug.Log("ending angle = "+endTan);

        Vector2 pureDir = new Vector2(dir.x,dir.y);
        foreach(Vector2Int test in Circle(center,radius+1))
        {
            int d = GridUtility.ManhattanDistance(test,center);
            if(d < radius && d > 0){
                float testTan = Mathf.Atan2(test.y-center.y,test.x-center.x);
                Debug.Log("manhattan distance:"+d+ " for test "+test +" with arctan2:"+testTan);
                if(startTan < endTan)
                {
                    if(testTan < endTan && testTan >= startTan)
                    {
                        arci.Add(test);
                    }
                }else
                {
                    if(testTan > endTan && testTan <= startTan)
                    {
                        arci.Add(test);
                    }
                }
                
            }
        }
        return arci.ToArray();
    }
    public static int CompareV2ByTopLeft(Vector2Int a, Vector2Int b)
    {
        if (a == null)
        {
            if (b == null)
            {
                // If x is null and y is null, they're
                // equal. 
                return 0;
            }
            else
            {
                // If x is null and y is not null, y
                // is greater. 
                return -1;
            }
        }
        else
        {
            // If x is not null...
            //
            if (b == null)
                // ...and y is null, x is greater.
            {
                return 1;
            }
            else
            {
                // ...and y is not null, compare them.
                int xDiff = a.x-b.x;
                xDiff = Mathf.Clamp(-1,1,xDiff);
                if (xDiff != 0)
                {
                    // If x values are different
  
                    return xDiff;
                }
                else
                {
                    //compare y diff
                    int yDiff = b.y-a.y;//should this be b-a?
                        xDiff = Mathf.Clamp(-1,1,yDiff);
                    return yDiff;
                }
            }
        }
    }
}
