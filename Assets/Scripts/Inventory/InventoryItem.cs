using System;
using UnityEngine;

namespace Inventory
{
	public class InventoryItem : ScriptableObject
	{
		public string itemName;
		public Sprite sprite;
		public bool LoseItemAfterUse;
		public Action useItemAction;
		public Action<InventoryItem> useInventoryItemAction;
		public void UseItem()
		{
			useItemAction?.Invoke();
			useInventoryItemAction?.Invoke(this);
		}
	}
}