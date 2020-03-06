using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public InventoryItemCollection inventory;
    public GameObject inventoryUIPrefab;
    private void OnEnable()
    {
        Debug.Log("Subscribing to listener");
        if (inventory.updateTrigger != null) inventory.updateTrigger += UpdateInventoryDisplay;
        UpdateInventoryDisplay();//also do this when we get re-enabled
    }

    private void OnDisable()
    {
        if (inventory.updateTrigger != null) inventory.updateTrigger -= UpdateInventoryDisplay;
    }
    
    void UpdateInventoryDisplay()
    {
        Debug.Log("Inventory display to be updated");
        //Destroy all children
        foreach (Transform child in transform)
        {
            Destroy((child.gameObject));
        }

        foreach (var item in inventory)
        {
            var button = GameObject.Instantiate(inventoryUIPrefab, transform).GetComponent<InventoryUIButton>();
            button.item = item;
        }
        
    }
}
