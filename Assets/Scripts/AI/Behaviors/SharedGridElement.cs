using BehaviorDesigner.Runtime;
using GameplayScripts;

[System.Serializable]
public class SharedGridElement : SharedVariable<GridElement>
{
	public static implicit operator SharedGridElement(GridElement value)
	{
		return new SharedGridElement {Value = value};
	}
}