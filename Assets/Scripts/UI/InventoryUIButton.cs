using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIButton : MonoBehaviour
{
    public InventoryItem item;

    void Start()
    {
        Image image = GetComponent<Image>();
        if (image != null) image.sprite = item.sprite;
        Text text = GetComponentInChildren<Text>();
        if (text != null) text.text = item.itemName;
    }
}
