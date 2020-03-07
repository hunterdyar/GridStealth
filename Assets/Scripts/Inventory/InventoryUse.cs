using ScriptableObjectArchitecture;
using UnityEngine;

namespace Inventory
{
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
}
