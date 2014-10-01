using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Strategies
{
	public class RandomStrategy: Strategy
	{
		public RandomStrategy(Map map) : base(map) 
		{
			this.Rnd = new Random(map.GetHashCode());
		}
		protected Random Rnd { get; set; }

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			return true;
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			var tile = this.Map[state.GetHero(playerToMove)];

			// a random move excluding stay.
			return tile.Directions[this.Rnd.Next(tile.Directions.Length -1 )];
		}
	}
}
