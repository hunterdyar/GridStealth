using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
public class GameFlowManager : ScriptableObject
{
    public GameFlowManagerSetup runner;
    public static GameFlowManager instance;
    public TilemapManager tilemapManager;
    public Action PostGridElementsUpdatedAction;
    public bool playerCanMove;//waiting for player input.
    List<AIBase> lumpAI;
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

        StartAITurn();

        //Update Lights, things that care that objects have moved.
        
        PostGridElementsUpdatedAction?.Invoke();
        
        //Update Lights, displayed.
        tilemapManager.UpdateBrightnessDisplay();
        //
        playerCanMove = true;
    }
    void StartAITurn()
    {
        runner.StartCoroutine(AIDoMove());
    }
    IEnumerator AIDoMove()
    {
        Debug.Log("AI Do Move");
        SortAI();
        List<TurnInfo> playerBlockingTurns = new List<TurnInfo>();
        foreach(AIBase ai in lumpAI)
        {
            TurnInfo info = ai.TakeTurn();//this takes the actual turn
            if(info.blockPlayerMovement){
                playerBlockingTurns.Add(info);
            }
            while(info.blockAIMovement){
                yield return null;
            }
        }
        //This loop wont finish until every player blocking turninfo is false.
        foreach(TurnInfo pbt in playerBlockingTurns)
        {
            while(pbt.blockPlayerMovement)
            {
                yield return null;
            }
        }

    }
    public void RegisterAI(AIBase ai)
    {
        lumpAI.Add(ai);
    }
    public void DeRegisterAI(AIBase ai)
    {
        if(lumpAI.Contains(ai)){
            lumpAI.Remove(ai);
        }
    }
    [Button("sort AI")]
    void SortAI()
    {
        lumpAI.Sort();
    }
    //

}
