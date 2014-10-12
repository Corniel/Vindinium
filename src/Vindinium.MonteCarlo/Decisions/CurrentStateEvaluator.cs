using Vindinium.Decisions;

namespace Vindinium.MonteCarlo.Decisions
{
	public class CurrentStateEvaluator : IStateEvaluator
	{
		public static readonly CurrentStateEvaluator Instance = new CurrentStateEvaluator();

		public ScoreCollection Evaluate(Map map, State state)
		{
			var s1 = new GoldScore((ushort)state.Hero1.Gold);
			var s2 = new GoldScore((ushort)state.Hero2.Gold);
			var s3 = new GoldScore((ushort)state.Hero3.Gold);
			var s4 = new GoldScore((ushort)state.Hero4.Gold);

			return new ScoreCollection(s1, s2, s3, s4);
		}
	}
}
