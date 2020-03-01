using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridElement))]
public class AIBase : MonoBehaviour
{
    public GameFlowManager gameFlowManager;

    public GridElement gridElement;
}
