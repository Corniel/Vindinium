using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Strategies
{
	public class SuicideStrategy : Strategy
	{
		public SuicideStrategy(Map map) : base(map) { }

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			var spwan = this.Map.GetSpawn(playerToMove);
			
			return PlayerTypes.Other[playerToMove]
				.Any(player => Map[state.GetHero(player)] == spwan);
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			throw new NotImplementedException();
		}
	}
}
