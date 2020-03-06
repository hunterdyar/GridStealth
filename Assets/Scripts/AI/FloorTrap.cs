using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FloorTrap : AIBase
{
	private SpriteRenderer spriteRenderer;
	public GridDirection pushDirection;
	[FoldoutGroup("Sprites")] public Sprite pushUpSprite;
	[FoldoutGroup("Sprites")] public Sprite pushDownSprite;
	[FoldoutGroup("Sprites")] public Sprite pushLeftSprite;
	[FoldoutGroup("Sprites")] public Sprite pushRightSprite;

	new void Awake()
	{
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetupSprite();
	}

	void SetupSprite()
	{
		if (pushDirection == GridDirection.left)
		{
			spriteRenderer.sprite = pushLeftSprite;
		}
		else if (pushDirection == GridDirection.right)
		{
			spriteRenderer.sprite = pushRightSprite;
		}
		else if (pushDirection == GridDirection.up)
		{
			spriteRenderer.sprite = pushUpSprite;
		}
		else if (pushDirection == GridDirection.down)
		{
			spriteRenderer.sprite = pushDownSprite;
		}
	}

	public override TurnInfo TakeTurn()
	{
		TurnInfo info = new TurnInfo(this);
		//check any objects that are here, and if they have playerInput (better player check?)
		//push em.
		List<GridElement> itemsHereNow = new List<GridElement>(gridElement.tileNode.itemsHere);
		//clone the list becuase we may modify it when we move things that are in the lsit
		foreach (GridElement ge in itemsHereNow)
		{
			if (ge.GetComponent<Agent>() != null)
			{
				Debug.Log("floor trap on " + ge.gameObject.name);
				ge.GetComponent<Agent>().Move(GridUtility.DirToV2(pushDirection), false); //false is not using up a turn when pushing
			}
		}

		return info;
	}
}