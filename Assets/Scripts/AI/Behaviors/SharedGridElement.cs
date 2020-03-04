using BehaviorDesigner.Runtime;
[System.Serializable]
public class SharedGridElement : SharedVariable<GridElement>
{
   public static implicit operator SharedGridElement(GridElement value) { return new SharedGridElement { Value = value }; }
}