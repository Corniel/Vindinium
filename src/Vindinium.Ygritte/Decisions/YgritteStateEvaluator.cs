using Vindinium.Decisions;

namespace Vindinium.Ygritte.Decisions
{
	public class YgritteStateEvaluator : IStateEvaluator
	{
		public static readonly YgritteStateEvaluator Instance = new YgritteStateEvaluator();

		public ScoreCollection Evaluate(Map map, State state)
		{
			var s1 = GetScore(map, state, PlayerType.Hero1);
			var s2 = GetScore(map, state, PlayerType.Hero2);
			var s3 = GetScore(map, state, PlayerType.Hero3);
			var s4 = GetScore(map, state, PlayerType.Hero4);

			return new ScoreCollection(s1, s2, s3, s4);
		}

		private PotentialScore GetScore(Map map, State state, PlayerType player)
		{
			var hero = state.GetHero(player);
			var gold = hero.Gold;
			var mines = hero.Mines;
			var health = hero.Health;

			var turnsLeft = (1200 - (int)player - state.Turn) >> 2;

			var score = 1000 * gold;
			score += 2 * (250 + (int)health) * (1 + mines) * turnsLeft;

			return new PotentialScore(score);
		}
	}
}
