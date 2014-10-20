using System;
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
		public Dictionary<PlayerType, int> Healths { get; protected set; }
		public IMineOwnership Mines { get; protected set; }
		public MoveDirection[] Directions { get; protected set; }
		public int Profit { get; protected set; }

		public SafePath ToSafePath(int turns, int profit)
		{
			return new SafePath(this.Directions, turns, profit);
		}

		public Dictionary<PlayerType, int> CloneHealths()
		{
			var healths = new Dictionary<PlayerType, int>()
			{
				{ PlayerType.Hero1, this.Healths[PlayerType.Hero1] },
				{ PlayerType.Hero2, this.Healths[PlayerType.Hero2] },
				{ PlayerType.Hero3, this.Healths[PlayerType.Hero3] },
				{ PlayerType.Hero4, this.Healths[PlayerType.Hero4] },
			};
			return healths;
		}

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Source: {0}, Turns: {7}, Healths: {1}, {2}, {3}, {4}, Profit: {5}, Directions: {6}",
					this.Source.DebugToString(),
					this.Healths[PlayerType.Hero1],
					this.Healths[PlayerType.Hero2],
					this.Healths[PlayerType.Hero3],
					this.Healths[PlayerType.Hero4],
					this.Profit,
					this.Directions == null ? "<none>" : String.Join(", ", this.Directions),
					this.Turns);
			}
		}

		public static PotentialPath Initial(Tile source, State state)
		{
			return new PotentialPath()
			{
				Source = source,
				Healths = new Dictionary<PlayerType, int>()
				{
					{ PlayerType.Hero1, state.GetHero(PlayerType.Hero1).Health },
					{ PlayerType.Hero2, state.GetHero(PlayerType.Hero2).Health },
					{ PlayerType.Hero3, state.GetHero(PlayerType.Hero3).Health },
					{ PlayerType.Hero4, state.GetHero(PlayerType.Hero4).Health },
				},
				Mines = state.Mines,
			};
		}
		public static PotentialPath CreateFollowUp(Tile source, Dictionary<PlayerType, int> healths, IMineOwnership mines, MoveDirection[] directions, int turns, int profit)
		{
			return new PotentialPath()
			{
				Source = source,
				Healths = healths,
				Mines = mines,
				Directions = directions,
				Turns = turns,
				Profit = profit,
			};
		}
	}
}
