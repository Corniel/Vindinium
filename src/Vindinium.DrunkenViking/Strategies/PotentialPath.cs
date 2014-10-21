using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Vindinium.DrunkenViking.Strategies
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class PotentialPath
	{
		public int Turns { get; protected set; }
		public Tile Source { get; protected set; }
		public int Health { get; protected set; }
		public IMineOwnership Mines { get; protected set; }
		public MoveDirection[] Directions { get; protected set; }
		public int Profit { get; protected set; }

		public SafePath ToSafePath(PlayerType player, int turns, int profit)
		{
			return new SafePath(this.Mines.Count(player), turns, profit, this.Directions.ToArray());
		}

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Source: {0}, Turns: {1}, Health: {2}, Profit: {3}, Directions: {4}",
					this.Source.DebugToString(),
					this.Turns,
					this.Health,
					this.Profit,
					this.Directions == null ? "<none>" : String.Join(", ", this.Directions));
			}
		}
		public override string ToString() { return this.DebuggerDisplay; }

		public static PotentialPath Initial(Tile source, State state)
		{
			return new PotentialPath()
			{
				Source = source,
				Health = state.GetActiveHero().Health,
				Mines = state.Mines,
			};
		}
		public static PotentialPath CreateFollowUp(Tile source, int health, IMineOwnership mines, MoveDirection[] directions, int turns, int profit)
		{
			return new PotentialPath()
			{
				Source = source,
				Health = health,
				Mines = mines,
				Directions = directions,
				Turns = turns,
				Profit = profit,
			};
		}
	}
}
