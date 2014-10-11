using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.MonteCarlo.Decisions;

namespace Vindinium.UnitTests.Decisions
{
	[TestFixture]
	public class AverageScoreDecisionTest
	{
		[Test]
		public void Create_3OptionsIn17Runs_West()
		{
			var scores = new Dictionary<MoveDirection, Dictionary<PlayerType, long>>()
			{
				{ MoveDirection.E, new Dictionary<PlayerType, long>()
					{
						{ PlayerType.Hero1, 12345 },
						{ PlayerType.Hero2, 80345 },
						{ PlayerType.Hero3, 45345 },
						{ PlayerType.Hero4, 45345 },
					}
				},
				{ MoveDirection.W, new Dictionary<PlayerType, long>()
					{
						{ PlayerType.Hero1, 12345 },
						{ PlayerType.Hero2, 80345 },
						{ PlayerType.Hero3, 45345 },
						{ PlayerType.Hero4, 71234 },
					}
				},
				{ MoveDirection.S, new Dictionary<PlayerType, long>()
					{
						{ PlayerType.Hero1, 1200 },
						{ PlayerType.Hero2, 1200 },
						{ PlayerType.Hero3, 1200 },
						{ PlayerType.Hero4, 1201 },
					}
				}
			};

			var act = AverageScoreDecision.Create(scores, 17 * 3, PlayerType.Hero4);

			foreach (var score in act.Scores)
			{
				Console.WriteLine(score.DebuggerDisplay);
			}

			var expMove = MoveDirection.W;
			var expScore = "h1: 726.17, h2: 4,726.17, h3: 2,667.35, h4: 4,190.23";

			Assert.AreEqual(expMove, act.Move);
			Assert.AreEqual(expScore, act.Score.DebuggerDisplay);
		}
	}
}
