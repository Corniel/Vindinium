namespace Vindinium.Decisions
{
	public interface IStateEvaluator
	{
		ScoreCollection Evaluate(Map map, State state);
	}
}
