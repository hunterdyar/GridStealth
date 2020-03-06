using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class GridElement : MonoBehaviour
{
	[Title("Configuration")] public TilemapManager tilemapManager;
	public TileNode tileNode;
	public bool solid = false;
	public bool opaque = false;
	[Title("Settings")] public int brightness;
	public Vector2Int position => GridPosition();
	public Action OnNewPositionAction;
	public Action<Vector2Int> SoundFromAction;

	public Vector2Int GridPosition()
	{
		return tilemapManager.WorldToCell(transform.position);
	}

	void Start()
	{
		OnNewPosition();
	}

	public void SoundFrom(Vector2Int source)
	{
		SoundFromAction?.Invoke(source);
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
		} //else: our object was moved where there isnt a tile. 

		if (OnNewPositionAction != null)
		{
			OnNewPositionAction();
		}
	}
}