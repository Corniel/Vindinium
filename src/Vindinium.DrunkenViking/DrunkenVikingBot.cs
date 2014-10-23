using System;
using System.Collections.Generic;
using System.Configuration;
using Vindinium.App;
using Vindinium.DrunkenViking.Strategies;

namespace Vindinium.DrunkenViking
{
	public class DrunkenVikingBot : Program<DrunkenVikingBot>
	{
		public static void Main(string[] args) { DrunkenVikingBot.DoMain(args); }

		public DrunkenVikingBot()
		{
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
			this.Strategies = new List<Strategy>();
		}

		protected List<Strategy> Strategies { get; set; }
		public TimeSpan Timout { get; protected set; }

		protected override void CreateGame()
		{
			base.CreateGame();

			this.Strategies.Clear();
			this.Strategies.Add(new SafePathStrategy(this.Map));
			this.Strategies.Add(new ToNearestSafeTaverneStrategy(this.Map));
			this.Strategies.Add(new ToNearestTaverneStrategy(this.Map));
			this.Strategies.Add(new ToNearestSafeMineStrategy(this.Map));
			this.Strategies.Add(new ToNearestMineStrategy(this.Map));

			foreach (var strategy in this.Strategies)
			{
				strategy.Initializes();
			}
		}

		public override Move GetMove()
		{
			UpdateState();

			var hero = this.State.GetHero(this.Player);
			var location = this.Map[hero];

			foreach (var strategy in this.Strategies)
			{
				if (strategy.Applies(this.State, location, hero, this.Player))
				{
					var direction = strategy.GetMove(this.State, location, hero, this.Player);
					var evaluation = String.Format("{0,4} Move: {1}, Gold: {2}, Strategy: {3}",
						this.State.Turn,
						direction,
						hero.Gold,
						strategy.GetType().Name.Replace("Strategy", ""));

					Console.WriteLine(evaluation);
					return new Move(direction, evaluation);
				}
			}
			return Move.None;
		}
	}
}
