using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Strategies
{
	public class ToTrippleMineStrategy : Strategy
	{
		public ToTrippleMineStrategy(Map map) : base(map) { }

		public override void Initializes()
		{
			
		}

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			return false;
			//return (state.GetHero(playerToMove).Health * 3) > Hero.HealthBattle;
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			throw new NotImplementedException();
		}
	}
}
