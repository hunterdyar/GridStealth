using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpriteDither : MonoBehaviour
{
    SpriteRenderer sr;
    public TileNode tileNode;
    public List<Sprite> ditherPatterns;
    float setBrightness;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(tileNode != null)
        {
            if(setBrightness != tileNode.brightness)
            {
                setBrightness = tileNode.brightness;
                int index = Mathf.FloorToInt(Mathf.Clamp01(1-setBrightness)*(ditherPatterns.Count-1));
                sr.sprite = ditherPatterns[index];
            }
        }
    }
}
