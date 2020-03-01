using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridItem : MonoBehaviour
{
    public Vector2Int position;
    public GridManager holdingGrid;
    public bool initializeOnAwake;
    public bool solid;//BLocks Line-Of-Sight
    public float brightness = 0;
    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        brightness = 0;
        if(initializeOnAwake)
        {
            Init();
        }
    }
    protected void Init()
    {
        position = new Vector2Int((int)transform.position.x,(int)transform.position.y);
        GridManager grid = GameObject.FindObjectOfType<GridManager>();
        grid.AddGridItem(position,this);
    }
    
    //Editor Testing
    void Update()
    {
        UpdateColor();
    }
    void UpdateColor()
    {
        if(solid)
        {
            sr.color = Color.blue;
        }else{
            sr.color = Color.Lerp(Color.black,Color.white,brightness/10);
        }
    }
    public int ManhattanDistanceTo(Vector2Int pos)
    {
        return GridManager.ManhattanDistance(position,pos);
    }
    public int ManhattanDistanceTo(GridItem gridItem)
    {
        return GridManager.ManhattanDistance(position,gridItem.position);
    }

    
}

