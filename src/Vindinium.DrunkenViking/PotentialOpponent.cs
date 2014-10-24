using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class PotentialOpponent
	{
		public PotentialOpponent(PlayerType player, int startTurn, int health, Tile location, bool isCrashed = false)
		{
			this.Player = player;
			this.StartTurn = startTurn;
			this.Health = health;
			this.Location = location;
			this.IsCrashed = isCrashed;
		}

		public PlayerType Player { get; protected set; }
		public int StartTurn { get; protected set; }
		public int Health { get; protected set; }
		public Tile Location { get; protected set; }
		public bool IsCrashed { get; protected set; }

		public bool StrongerAtLocationAndTime(Map map, Tile location, int health, int turn)
		{
			// The opponent does not exist yet.
			if (this.StartTurn > turn) { return false; }

			var spawn = map.GetSpawn(this.Player);

			// If a bot is crashed and own its spawn, always stronger.
			if (this.IsCrashed &&
				(spawn == this.Location || spawn == location) &&
				Map.GetManhattanDistance(this.Location, location) == 1)
			{
				return true;
			}

			var distancesToLocation = map.GetDistances(location);
			var distanceToLocation = this.StartTurn + (int)distancesToLocation.Get(this.Location);

			// The opponent can not reach the location in time.
			if (distanceToLocation - 1 > turn) { return false; }

			var oppoHealth = Math.Max(Hero.HealthMin, this.Health - turn + this.StartTurn);
			var oppoHits = 1 + (oppoHealth - 1) / Hero.HealthBattle;
			var hits = 1 + (health - 1) / Hero.HealthBattle;

			foreach (var neighbor in location.Neighbors)
			{
				var dis =distancesToLocation.Get(neighbor);
				if (dis == Distance.Unknown) { continue; }

				distanceToLocation = (int)dis;

				if (distanceToLocation >= turn)
				{
					if (spawn == this.Location || spawn == location)
					{
						return true;
					}
					if (distanceToLocation > turn) { oppoHits++; }

					if (oppoHits > hits)
					{
						return true;
					}
				}
			}
			return false;
		}

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return String.Format("Hero{0}, Turn: {1}, Health: {2}, Loc: ({3}, {4}){5}",
					(int)this.Player,
					this.StartTurn,
					this.Health,
					this.Location.X, this.Location.Y,
					this.IsCrashed ? ", IsCrashed" : "");

			}
		}
		public override string ToString() { return this.DebuggerDisplay; }

		public static IEnumerable<PotentialOpponent> Create(Map map, State state)
		{
			foreach (var player in PlayerTypes.Other[state.PlayerToMove])
			{
				var hero = state.GetHero(player);
				var health = hero.Health;
				var source = map[hero];

				yield return new PotentialOpponent(player, 0, hero.Health, source, hero.IsCrashed);

				if (!hero.IsCrashed)
				{
					foreach (var target in map.TaverneNeighbors)
					{
						var time = (int)map.GetDistances(target).Get(source);

						var healthNew = Math.Max(Hero.HealthMin, health - time);

						while(healthNew < Hero.HealthMax)
						{
							time++;
							healthNew = Math.Max(Hero.HealthMin, healthNew - 1);
							healthNew = Math.Min(Hero.HealthMax, healthNew + Hero.HealthTavern);
							yield return new PotentialOpponent(player, time, healthNew, target);
						}
					}
				}
			}
		}
	}
}
