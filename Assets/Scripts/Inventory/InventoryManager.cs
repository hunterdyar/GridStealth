using System.Collections.Generic;
using Ludiq.PeekCore.CodeDom;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Inventory
{
	public class InventoryManager : MonoBehaviour
	{
		public InventoryItemCollection inventory;
		public InventoryItemCollection startingInventory;
		
		private TilemapManager _tilemapManager;
		public GameFlowManager gfm;
		private GridElement _gridElement;
	
		private void Awake()
		{
			_gridElement = GetComponent<GridElement>();
			_tilemapManager = _gridElement.tilemapManager;
			inventory.updateTrigger += InventoryCheck;
		}

		private void InventoryCheck()
		{
			//uh, so, the action only seems to get fired when it had a debug listener
			//so that's what this function is really doing. 
			//i guess i dont understand actions well enough?
			//or its a race-condition issue. idk.
		}
		void Start()
		{
			inventory.Clear();
			//reset starting inventory
			foreach (var item in startingInventory)
			{
				inventory.Add(item);
				
			}
			inventory.updateTrigger?.Invoke();
		}
		

		public bool HasItem(InventoryItem item)
		{
			return inventory.Contains(item);
		}

		private void OnEnable()
		{
			_gridElement.onNewPositionAction += CheckForItem;
		}

		private void AddItemToInventory(InventoryItem item)
		{
			if (!inventory.Contains(item))
			{
				inventory.Add(item);
				inventory.updateTrigger?.Invoke();
			}
		}

		private void RemoveItemFromInventory(InventoryItem item)
		{
			if (inventory.Contains(item))
			{
				inventory.Remove(item);
				inventory.updateTrigger?.Invoke();
			}
		}

		private void CheckForItem()
		{
			var pos = _gridElement.position;
			var tile = _tilemapManager.GetTileNode(pos);
			var collected = new List<GridElement>();
			for (int i = tile.itemsHere.Count;i>0;i--)
			{
				GridElement ge = tile.itemsHere[i-1];
				if (ge.item != null)
				{
					AddItemToInventory(ge.item);
					tile.itemsHere.Remove(ge);
					if (ge.destroyOnItemPickup)
					{
						Destroy(ge.gameObject);
					}
				}
			}
		}
	}
}