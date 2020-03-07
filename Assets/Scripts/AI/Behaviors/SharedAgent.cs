using BehaviorDesigner.Runtime;
using Gameplay;

[System.Serializable]
public class SharedAgent : SharedVariable<Agent>
{
	public static implicit operator SharedAgent(Agent value)
	{
		return new SharedAgent {Value = value};
	}
}