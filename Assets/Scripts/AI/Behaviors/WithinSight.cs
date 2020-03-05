using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("Grid System")]
public class WithinSight : Conditional
{
    public GridElement myGridElement;
    public Agent agent;
    public int distance;
    public float fieldOfViewAngle;
    public SharedGridElement targetGE;
    public SharedAgent targetAgent;
    public Vector2Int targetPosition;

    public override TaskStatus OnUpdate()
    {
        Vector2Int target = targetPosition;
        if(targetGE.Value != null)
        {
            target = targetGE.Value.position;
        }
        if(targetAgent.Value != null)
        {
            target = targetAgent.Value.position;
        }

        if (WithinViewCone(target, fieldOfViewAngle)) {
            return TaskStatus.Success;
        }
        
        return TaskStatus.Failure;
    }
    public bool WithinViewCone(Vector2Int target, float fieldOfViewAngle)
    {
        Vector2Int direction = target - myGridElement.position;
        fieldOfViewAngle = fieldOfViewAngle/2;
        if(GridUtility.ManhattanDistance(target,myGridElement.position) > distance){return false;}
        return Vector2.Angle((Vector2)direction, (Vector2)agent.facingDirection) <= fieldOfViewAngle;
    }
}
