using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.Decisions;
using Vindinium.Ygritte2.Decisions;

namespace Vindinium.Ygritte2.Evaluation
{
	public class FourMinesEvaluator : Evaluator
	{
		private Map Map { get; set; }
		private Distance[,] MineDistances { get; set; }

		public override ScoreCollection Evaluate(State state)
		{
			var s1 = GetScore(state, PlayerType.Hero1);
			var s2 = GetScore(state, PlayerType.Hero2);
			var s3 = GetScore(state, PlayerType.Hero3);
			var s4 = GetScore(state, PlayerType.Hero4);

			return PotentialScore.CreateCollection(s1, s2, s3, s4);
		}

		private int GetScore(State state, PlayerType player)
		{
			var turn = state.Turn;
			var hero = state.GetHero(player);

			var score = hero.Gold * 1000;
			score += ((1195 - turn + (int)player) >> 2) * 950;

			var mines = hero.Mines;

			if(mines == 0)
			{
				var distance = MineDistances.Get(hero);
				score -= (int)distance * 1100;
			}
			if (mines > 1)
			{
				score += (mines - 1) * 2000;
			}
			return score;
		}

		public static FourMinesEvaluator Create(Map map)
		{
			var evaluator = new FourMinesEvaluator();
			evaluator.Map = map;
			evaluator.MineDistances = map.GetDistances(map.Mines);

			return evaluator;
		}
	}
}
