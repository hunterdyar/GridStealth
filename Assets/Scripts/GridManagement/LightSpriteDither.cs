using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
public class LightSpriteDither : MonoBehaviour
{
    SpriteRenderer sr;
    public ColorReference darkColor;
    public ColorReference lightColor;
    public TileNode tileNode;
    Sprite solidColor;
    public List<Sprite> ditherPatterns;
    float setBrightness;
    bool blinking;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = darkColor.Value;
        solidColor = ditherPatterns[ditherPatterns.Count-2];
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
    public void Blink(int color)
    {
        if(blinking){return;}
        if(color == 1)
        {
            BlinkLight();
        }else if(color == 0)
        {
            BlinkDark();
        }
    }
    public void BlinkLight(){
        StartCoroutine(BlinkColor(lightColor.Value));
    }
    public void BlinkDark()
    {
        StartCoroutine(BlinkColor(darkColor.Value));
    }
    IEnumerator BlinkColor(Color c)
    {
        blinking = true;
        Color normalColor = sr.color;
        Sprite normalSprite = sr.sprite;
        sr.color = c;
        sr.sprite = solidColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = normalColor;
        sr.sprite = normalSprite;
        blinking = false;
    }
}
