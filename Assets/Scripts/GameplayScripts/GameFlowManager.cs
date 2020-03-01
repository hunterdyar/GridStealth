using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
public class GameFlowManager : ScriptableObject
{
    public static GameFlowManager instance;
    public TilemapManager tilemapManager;
    public Action PostGridElementsUpdatedAction;
    public bool playerCanMove;//waiting for player input.

    [Button("Set Singleton")]
    public void SetSingleton(){instance = this;}
    public void Init()
    {
        playerCanMove = true;
    }
    public void PlayerTookTurn()
    {
        playerCanMove = false;
     
        //AIAgents to take their turn

        //

        //Update Lights, things that care that objects have moved.
        if(PostGridElementsUpdatedAction != null)
        {
            PostGridElementsUpdatedAction.Invoke();
        }
        //Update Lights, displayed.
        tilemapManager.UpdateBrightnessDisplay();
        //
        playerCanMove = true;
    }

}
