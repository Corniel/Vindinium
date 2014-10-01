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

		/// <summary>Gets the map.</summary>
		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public PlayerType Player { get; set; }

		public TimeSpan Timout { get; protected set; }

		protected override void CreateGame()
		{
			this.Strategies.Clear();
			this.Map = Map.Parse(this.Client.Response.game.board.ToRows());
			this.State = State.Create(this.Map);
			this.Player = this.Client.Response.hero.Player;

			this.Strategies.Add(new ToNearestSafeTaverneStrategy(this.Map));
			this.Strategies.Add(new ToNearestTaverneStrategy(this.Map));
			this.Strategies.Add(new ToNearestSafeMineStrategy(this.Map));
			this.Strategies.Add(new ToNearestMineStrategy(this.Map));

			foreach (var strategy in this.Strategies)
			{
				strategy.Initializes();
			}
		}

		protected override MoveDirection GetMove()
		{
			this.State = this.State.Update(this.Client.Response.game);

			var hero = this.State.GetHero(this.Player);
			var location = this.Map[hero];

			foreach (var strategy in this.Strategies)
			{
				if (strategy.Applies(this.State, location, hero, this.Player))
				{
					var move = strategy.GetMove(this.State, location, hero, this.Player);
					Console.WriteLine("{0,4} Move: {1}, Gold: {2}, Strategy: {3}",
						this.State.Turn,
						move,
						hero.Gold,
						strategy.GetType().Name.Replace("Strategy", ""));

					return move;
				}
			}
			return MoveDirection.x;
		}
	}
}
