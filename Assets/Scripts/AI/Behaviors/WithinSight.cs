using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class WithinSight : Conditional
{
    public GridElement myGridElement;
    public Agent agent;
    public int distance;
    public float fieldOfViewAngle;
    public SharedGridElement target;

    public override TaskStatus OnUpdate()
    {
            if (WithinViewCone(target.Value, fieldOfViewAngle)) {
                return TaskStatus.Success;
            }
        
        return TaskStatus.Failure;
    }
    public bool WithinViewCone(GridElement target, float fieldOfViewAngle)
    {
        Vector2Int direction = target.position - myGridElement.position;
        if(direction.magnitude > distance){return false;}
        return Vector2.Angle((Vector2)direction, (Vector2)agent.facingDirection) < fieldOfViewAngle;
    }
}
