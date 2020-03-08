using Gameplay;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class InventoryUIButton : MonoBehaviour
    {
        public InventoryItem item;
        public Image image;
        public Sprite emptyItemSprite;
        public Text text;
        public GameFlowManager gfm;
        void Awake()
        {
            image = GetComponent<Image>();
            text = GetComponentInChildren<Text>();
            UpdateSelf();
        }

        public void UseItemButton()
        {
            if (item == null) return;
            item.UseItem();
            gfm.PlayerTookTurn();
        }
        public void UpdateSelf()
        {
            
            if (item == null)
            {
                if (emptyItemSprite != null)
                {
                    image.sprite = emptyItemSprite;
                }
                else
                {
                    image.sprite = null;
                }

                text.text = "empty";
            }
            else
            {
                if (image != null) image.sprite = item.sprite;
                if (text != null) text.text = item.itemName;
            }
        
        }
    }
}
