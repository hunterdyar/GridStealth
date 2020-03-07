using UnityEngine;

namespace Gameplay
{
	public class GameFlowManagerSetup : MonoBehaviour
	{
		public GameFlowManager manager;

		void Awake()
		{
			manager.runner = this;
			GameFlowManager.instance = manager;
			manager.Init();
		}
	}
}