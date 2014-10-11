using System.Diagnostics;
using Vindinium.Decisions;

namespace Vindinium.MonteCarlo.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class AverageScoreCollection
	{
		public MoveDirection Move { get; protected set; }
		public ScoreCollection Scores { get; protected set; }

		public string DebuggerDisplay { get { return string.Format("{0} {1}", Move, Scores.DebuggerDisplay); } }

		public static AverageScoreCollection Create(MoveDirection move, long avg1, long avg2, long avg3, long avg4, int runs)
		{
			var scores = new ScoreCollection(
				new AverageScore(avg1, runs),
				new AverageScore(avg2, runs),
				new AverageScore(avg3, runs),
				new AverageScore(avg4, runs));

			return new AverageScoreCollection()
			{
				Move = move,
				Scores = scores,
			};
		}
	}
}
