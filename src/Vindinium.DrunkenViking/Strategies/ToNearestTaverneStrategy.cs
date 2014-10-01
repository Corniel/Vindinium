using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Strategies
{
	public class ToNearestTaverneStrategy : Strategy
	{
		public ToNearestTaverneStrategy(Map map) : base(map) { }

		public override void Initializes()
		{
			this.DistanceToTaverne = Map.GetDistances(this.Map.Tavernes);
		}

		public Distance[,] DistanceToTaverne { get; protected set; }

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			return hero.Health <= Hero.HealthBattle ||
				(this.DistanceToTaverne.Get(location) < 2 && hero.Health < 80);
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			var distance = this.DistanceToTaverne.Get(location);

			foreach (var dir in location.Directions)
			{
				if (this.DistanceToTaverne.Get(location[dir]) < distance)
				{
					return dir;
				}
			}
			return MoveDirection.x;
		}
	}
}
