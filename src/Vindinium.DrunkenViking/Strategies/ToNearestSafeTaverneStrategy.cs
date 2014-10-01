using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Strategies
{
	public class ToNearestSafeTaverneStrategy : Strategy
	{
		public ToNearestSafeTaverneStrategy(Map map) : base(map) { }

		public MoveDirection Candidate { get; protected set; }

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			if (hero.Health >= Hero.HealthBattle)
			{
				return false;
			}

			this.Candidate = MoveDirection.x;

			var tavernes = Map.Tavernes;
			var blocked = PlayerTypes.Other[playerToMove]
					.Where(p => !state.GetHero(p).IsCrashed)
					.Select(p => this.Map[state.GetHero(p)])
					.SelectMany(t => t.Targets);


			var distances = Map.GetDistances(tavernes, blocked);
			var distance = distances.Get(location);

			foreach (var dir in location.Directions)
			{
				var test = distances.Get(location[dir]);
				if (test < distance && test < Distance.Blocked)
				{
					this.Candidate = dir;
					break;
				}
			}
			return Candidate != MoveDirection.x;
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			return this.Candidate;
		}
	}
}
