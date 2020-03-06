using BehaviorDesigner.Runtime;

[System.Serializable]
public class SharedFOV : SharedVariable<FOV>
{
	public static implicit operator SharedFOV(FOV value)
	{
		return new SharedFOV {Value = value};
	}
}