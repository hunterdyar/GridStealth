using BehaviorDesigner.Runtime;
[System.Serializable]
public class SharedTilemapManager : SharedVariable<TilemapManager>
{
   public static implicit operator SharedTilemapManager(TilemapManager value) { return new SharedTilemapManager { Value = value }; }
}