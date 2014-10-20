
namespace Vindinium.DrunkenViking.Strategies
{
	public class SafePathStrategy : Strategy
	{
		public SafePathStrategy(Map map) : base(map) { }

		public SafePathCollection Collection { get; protected set; }

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			this.Collection = SafePathCollection.Create(this.Map, state);
			this.Collection.Procces();
			return this.Collection.BestPath != null;
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			return this.Collection.BestMove;
		}
	}
}
