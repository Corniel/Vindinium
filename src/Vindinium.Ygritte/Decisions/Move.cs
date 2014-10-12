namespace Vindinium.Ygritte.Decisions
{
	public abstract class Move
	{
		public static readonly MoveCrashed Crashed = new MoveCrashed();
		public static readonly MoveStay Stay = new MoveStay();

		public abstract Tile GetTarget(Tile source, Map map, State state);
	}
}
