using Vindinium.App;

namespace Vindinium.Random
{
	public class RandomBot : Program<RandomBot>
	{
		public RandomBot()
		{
			this.Rnd = new System.Random(69);
		}

		public System.Random Rnd { get; protected set; }

		public static void Main(string[] args)
		{
			RandomBot.DoMain(args);
		}

		public override Move GetMove()
		{
			UpdateState();

			var location = this.Map[this.State.GetHero(this.Player)];
			return new Move(location.Directions[this.Rnd.Next(location.Directions.Length)]);
		}
	}
}
