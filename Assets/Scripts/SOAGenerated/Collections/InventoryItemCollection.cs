using System;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "InventoryItemCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "InventoryItem",
	    order = 120)]
	public class InventoryItemCollection : Collection<InventoryItem>
	{
		[FormerlySerializedAs("UpdateTrigger")] public Action updateTrigger;

		public void UpdateListeners()
		{
			updateTrigger?.Invoke();
		}
	}
}