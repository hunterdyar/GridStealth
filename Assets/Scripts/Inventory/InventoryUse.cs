using System;
using System.Collections.Generic;
using Gameplay;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Inventory
{
	public class InventoryUse : MonoBehaviour
	{
		[Title("Settings")]
		public InventoryItemCollection inventory;
		public Agent playerAgent;
		public TilemapManager tilemapManager;
		public InventoryManager inventoryManager;
		[Title("Knife Setup")]
		public InventoryItem knifeItem;
		
		[Title("Flashlight Setup")]
		public InventoryItem flashLightItem;
		public GridLight flashlight;

		[Title("Bell Setup")] public InventoryItem bellItem;
		void Awake()
		{
			playerAgent = GetComponent<Agent>();
		}

		
		void OnEnable()
		{
			knifeItem.useItemAction += Stab;
			flashLightItem.useItemAction += ToggleLightSwitch;
			bellItem.useItemAction += RingBell;
		}

		private void OnDisable()
		{
			knifeItem.useItemAction -= Stab;
			flashLightItem.useItemAction -= ToggleLightSwitch;
			bellItem.useItemAction -= RingBell;
		}
		public void UseItem(InventoryItem item)
		{
			if (item == knifeItem)
			{
				Stab();
			}else if (item == bellItem)
			{
				RingBell();
			}else if (item == flashLightItem)
			{
				ToggleLightSwitch();
			}
		}

		private void Stab()
		{
			var stabLocation = playerAgent.position + playerAgent.facingDirection;
			tilemapManager.SoundOneTile(stabLocation);
			//
			TileNode stabNode = tilemapManager.GetTileNode(stabLocation);
			List<AIBase> killme = new List<AIBase>();
			foreach (var ge in stabNode.itemsHere)
			{
				if (ge.GetComponent<AIBase>() != null)
				{
					killme.Add(ge.GetComponent<AIBase>());
				}
			}

			foreach (var toDie in killme)
			{
				toDie.KillMe();
			}
		}
		
		private void ToggleLightSwitch()
		{
			flashlight.lightOn = !flashlight.lightOn;
			flashlight.ForceIlluminate();
		}

		private void RingBell()
		{
			tilemapManager.Sound(playerAgent.position,5);
		}
		

	}
}
