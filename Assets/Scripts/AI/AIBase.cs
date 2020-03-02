using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class AIBase : MonoBehaviour
{
    public GameFlowManager gameFlowManager;

    public GridElement gridElement;

    public void OnEnable()
    {
        gridElement = GetComponent<GridElement>();
        gameFlowManager.RegisterAI((AIBase)this);
    }
    public void OnDisable()
    {
        gameFlowManager.DeRegisterAI((AIBase)this);
    }
}
