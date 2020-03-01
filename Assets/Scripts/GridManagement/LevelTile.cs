using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// [CreateAssetMenu(fileName = "litTile",menuN)]
public class LevelTile : Tile
{
    public bool solid;
    public bool opaque;
    public Vector2Int position;

    // public List<GridLight> litBy = new List<GridLight>();
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        position = (Vector2Int)location;
        tileData.sprite = this.sprite;
        tileData.color = this.color;
        tileData.transform = this.transform;
        tileData.gameObject = this.gameObject;
        tileData.flags = this.flags;
        tileData.colliderType = this.colliderType;
    }
    public int ManhattanDistanceTo(Vector2Int pos)
    {
        return GridUtility.ManhattanDistance(position,pos);
    }
    public int ManhattanDistanceTo(LevelTile gridItem)
    {
        return GridUtility.ManhattanDistance(position,gridItem.position);
    }
}
