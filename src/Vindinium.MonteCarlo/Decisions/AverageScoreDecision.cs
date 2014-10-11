using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vindinium.Decisions;

namespace Vindinium.MonteCarlo.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class AverageScoreDecision
	{
		public AverageScoreDecision()
		{
			this.Scores = new List<AverageScoreCollection>();
		}
		public List<AverageScoreCollection> Scores { get; protected set; }
		public ScoreCollection Score { get { return this.Scores[0].Scores; } }
		public MoveDirection Move { get { return this.Scores[0].Move; } }

		public string DebuggerDisplay { get { return string.Format("{0} {1}", Move, Score.DebuggerDisplay); } }
		public string ToConsoleDisplay(PlayerType player)
		{
			var str = DebuggerDisplay;
			str = ScoreCollection.ToConsoleDisplay(str, player);
			return str;
		}

		public static AverageScoreDecision Create(Dictionary<MoveDirection, Dictionary<PlayerType, long>> scores, int runs, PlayerType player)
		{
			runs /= scores.Count;
			if (runs == 0) { runs = 1; }

			var decision = new AverageScoreDecision();

			foreach (var move in scores.Keys)
			{
				var values = scores[move];
				var col = AverageScoreCollection.Create(
					move,
					values[PlayerType.Hero1],
					values[PlayerType.Hero2],
					values[PlayerType.Hero3],
					values[PlayerType.Hero4],
					runs);

				decision.Scores.Add(col);
			}
			decision.Scores = decision.Scores.OrderByDescending(sc => sc.Scores.Get(player).ToUInt32()).ToList();
			return decision;
		}
	}
}
