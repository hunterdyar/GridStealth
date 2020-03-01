using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode
{
    public Vector2Int position;
    public bool solid;
    public int brightness = 0;
    public GameObject gameObjectHere;
    public List<GridLight> lightsOnMe = new List<GridLight>();

    public void SetBrightness()
    {
        brightness = 0;
        foreach(GridLight l in lightsOnMe)
        {
            brightness += l.BrightnessForTile(position);
        }
    }
}
