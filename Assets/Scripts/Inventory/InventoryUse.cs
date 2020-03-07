using System.Collections;
using System.Collections.Generic;
using Inventory;
using ScriptableObjectArchitecture;
using UnityEngine;

public class InventoryUse : MonoBehaviour
{
	public InventoryItemCollection inventory;

	public InventoryItem knifeItem;
	public InventoryItem flashLightItem;

	
	public void UseItem(InventoryItem item)
	{
		if (item == knifeItem)
		{
			Stab();
		}
	}

	public void Stab()
	{
		//STABBY STAB STAB
	}

}
