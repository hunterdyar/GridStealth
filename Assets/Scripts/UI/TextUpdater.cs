using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
	public GameFlowManager gameFlowManager;
	public Text text;

	void Update()
	{
		if (gameFlowManager.playerCanMove.Value)
		{
			text.text = (gameFlowManager.playerTurnsAllowed.Value - gameFlowManager.playerTurnsTaken.Value).ToString();
		}
		else
		{
			text.text = "-";
		}
	}
}