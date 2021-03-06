﻿using System;
using Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class GridElement : MonoBehaviour
	{
		[Title("Configuration")] public TilemapManager tilemapManager;
		public TileNode tileNode;
		public bool solid = false;
		public bool opaque = false;
		[Title("Settings")] public int brightness;
		public Vector2Int position => tilemapManager.WorldToCell(transform.position);
		[FormerlySerializedAs("OnNewPositionAction")] public Action onNewPositionAction;
		public Action<Vector2Int> soundFromAction;
		public InventoryItem item;
		public bool destroyOnItemPickup = true;
		
		void Start()
		{
			OnNewPosition();
		}

		public void SoundFrom(Vector2Int source)
		{
			soundFromAction?.Invoke(source);
		}

		public void OnNewPosition()
		{
			//tell the previous tileNode and current tileNode if we are solid.
			if (tileNode != null)
			{
				if (tileNode.itemsHere.Contains(this))
				{
					tileNode.itemsHere.Remove(this);
				}
			}

			tileNode = tilemapManager.GetTileNode(position);
			if (tileNode != null)
			{
				if (!tileNode.itemsHere.Contains(this))
				{
					tileNode.itemsHere.Add(this);
				}
				else
				{
					Debug.LogWarning("tile node already had gridElement. This should never happen.");
				}
			} //else: our object was moved where there isn't a tile. 

		
			onNewPositionAction?.Invoke();
		}
	}
}