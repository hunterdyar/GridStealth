using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManagerSetup : MonoBehaviour
{
    public GameFlowManager manager;
    void Awake()
    {
        manager.runner = this;
        GameFlowManager.instance = manager;
        manager.Init();        
        
    }

}
