using Vindinium.Decisions;
using Vindinium.Ygritte2.Decisions;

namespace Vindinium.Ygritte2.Evaluation
{
	public class Evaluator
	{
		public virtual ScoreCollection Evaluate(State state)
		{
			var h1 = state.GetHero(PlayerType.Hero1);
			var h2 = state.GetHero(PlayerType.Hero2);
			var h3 = state.GetHero(PlayerType.Hero3);
			var h4 = state.GetHero(PlayerType.Hero4);

			var s1 = h1.Gold * 1000;
			var s2 = h2.Gold * 1000;
			var s3 = h3.Gold * 1000;
			var s4 = h4.Gold * 1000;

			s1 += h1.Mines * 800;
			s2 += h2.Mines * 800;
			s3 += h3.Mines * 800;
			s4 += h4.Mines * 800;

			//s1 += (int)h1.Health * 5;
			//s2 += (int)h2.Health * 5;
			//s3 += (int)h3.Health * 5;
			//s4 += (int)h4.Health * 5;

			return PotentialScore.CreateCollection(s1, s2, s3, s4);
		}


		public static Evaluator Get(Map map)
		{
			if (map.Mines.Length == 4)
			{
				return FourMinesEvaluator.Create(map);
			}
			return new Evaluator();
		}
	}
}
