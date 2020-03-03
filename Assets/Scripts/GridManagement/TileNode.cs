using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode
{
    public Vector2Int position;
    public bool isSolid;//assigned by tile
    public bool isOpaque;
    public bool solid { get{return IsSolid();}}//currently solid.
    public bool opaque { get{return IsOpaque();}}
    public float brightness = 0;
    public GameObject gameObjectHere;
    //things that are on this square.
    public List<GridElement> itemsHere = new List<GridElement>();
    //lights that illuminate this node.
    public List<GridLight> lightsOnMe = new List<GridLight>();
    public LightSpriteDither dither;

    public void SetBrightness()
    {
        brightness = 0;
        foreach(GridLight l in lightsOnMe)
        {
            brightness += l.BrightnessForTile(position);
        }
    }
    public void SoundFrom(Vector2Int source)
    {
        foreach(GridElement item in itemsHere)
        {
            item.SoundFrom(source);
        }
    }
    public bool IsSolid(){
        if(isSolid){return true;}//assignedSolid overrides any elements that are ( but shouldnt be?) here
        if(itemsHere.Count>0)
        {
            foreach(GridElement ge in itemsHere)
            {
                if(ge.solid){return true;}
            }
            return false;
        }else{
            return false;//isSolid is false and no items here
        }
    }
    public bool IsOpaque(){
        if(isOpaque){return true;}//assignedSolid overrides any elements that are ( but shouldnt be?) here
        if(itemsHere.Count>0)
        {
            foreach(GridElement ge in itemsHere)
            {
                if(ge.opaque){return true;}
            }
            return false;
        }else{
            return false;//isSolid is false and no items here
        }
    }
}
