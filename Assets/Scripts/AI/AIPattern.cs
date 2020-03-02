using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternLoopType{
    loop,
    pingPong
}

[RequireComponent(typeof(Agent))]
public class AIPattern : AIBase
{
    Agent agent;
    public int startingIndex = 0;
    public PatternLoopType patternLoopType;
    public List<GridDirection> movementPattern;
    private int currentIndex;
    int patternDir = 1;
    new void Awake()
    {
        base.Awake();
        agent = GetComponent<Agent>();
        currentIndex = startingIndex;
        patternDir = 1;
    }
    Vector2Int GetDirection()
    {
        if(currentIndex >= movementPattern.Count)
        {
            if(patternLoopType == PatternLoopType.loop)
            {
                currentIndex = 0;
            }else if(patternLoopType == PatternLoopType.pingPong)
            {
                patternDir = -1;
                currentIndex = movementPattern.Count - 1;
            }
        }else if(currentIndex == -1){
            if(patternLoopType == PatternLoopType.loop)
            {
                currentIndex = movementPattern.Count-1;
            }else if(patternLoopType == PatternLoopType.pingPong)
            {
                currentIndex = 0;
                patternDir = 1;
            }
        }
        return GridUtility.DirToV2(movementPattern[currentIndex]);
    }
    public override TurnInfo TakeTurn()
    {
        Vector2Int dir = GetDirection();
        TurnInfo info = new TurnInfo(this);
        currentIndex = currentIndex + patternDir;
        return agent.Move(dir);
    }
}
