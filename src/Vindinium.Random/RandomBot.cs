using Vindinium.App;

namespace Vindinium.Random
{
	public class RandomBot : Program<RandomBot>
	{
		public RandomBot()
		{
			this.Rnd = new System.Random();
		}

		public System.Random Rnd { get; protected set; }

		public static void Main(string[] args)
		{
			RandomBot.DoMain(args);
		}

		protected override MoveDirection GetMove()
		{
			UpdateState();

			var location = this.Map[this.State.GetHero(this.Player)];
			return location.Directions[this.Rnd.Next(location.Directions.Length)];
		}
	}
}
