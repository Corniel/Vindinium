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
			return (MoveDirection)Rnd.Next(0, 5);
		}
	}
}
