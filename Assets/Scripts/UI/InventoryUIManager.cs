using ScriptableObjectArchitecture;
using UnityEngine;

namespace UI
{
    public class InventoryUIManager : MonoBehaviour
    {
        public InventoryItemCollection inventory;
        public InventoryUIButton[] inventorySlots;
        private void OnEnable()
        {
            if (inventory.updateTrigger != null) inventory.updateTrigger += UpdateInventoryDisplay;
            UpdateInventoryDisplay();//also do this when we get re-enabled, because, usually, inventory changes happen when the UI is hidden. like... when we move.
        }

        private void OnDisable()
        {
            if (inventory.updateTrigger != null) inventory.updateTrigger -= UpdateInventoryDisplay;
        }
    
        void UpdateInventoryDisplay()
        {
            for(int i = 0;i<inventorySlots.Length;i++)
            {
                inventorySlots[i].item = null;
                if (inventory.Count > i)
                {
                    if (inventory[i] != null)
                    {
                        inventorySlots[i].item = inventory[i];
                    }
                }
                inventorySlots[i].UpdateSelf();
            }
        }
    }
}
