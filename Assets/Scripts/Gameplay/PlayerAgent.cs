using BehaviorDesigner.Runtime;

namespace Gameplay
{
	public class PlayerAgent : Agent
	{
		public GameFlowManager gfm;
		public SharedVector2Int sharedPosition;

		new void Awake()
		{
			status = AgentStatus.Player;
			base.Awake();
		}
		protected void Start()
		{
			var playerAgent = new SharedAgent {Value = this};
			GlobalVariables.Instance.SetVariable("playerAgent", playerAgent);
			sharedPosition.Value = position;
		}

		protected override void MoveEnded(TurnInfo turn)
		{
			sharedPosition.Value = position;
			GlobalVariables.Instance.SetVariable("playerPosition", sharedPosition);
			base.MoveEnded(turn);
			if (turn.useUpTurn)
			{
				gfm.PlayerTookTurn();
			}
		}

		public void WaitATurn()
		{
			gfm.PlayerTookTurn();
		}
	}
}