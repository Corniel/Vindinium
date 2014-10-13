using System;
using System.Configuration;
using Vindinium.App;
using Vindinium.Ygritte.Decisions;

namespace Vindinium.Ygritte
{
	public class YgritteBot : Program<YgritteBot>
	{
		/// <summary>The main entry point of the program.</summary>
		public static void Main(string[] args) { YgritteBot.DoMain(args); }

		/// <summary>Constructor.</summary>
		public YgritteBot()
		{
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
		}

		/// <summary>Gets the time-out for the bot.</summary>
		public TimeSpan Timout { get; protected set; }

		/// <summary>Creates a game.</summary>
		protected override void CreateGame()
		{
			base.CreateGame();
			Node.Lookup.Clear();
		}

		/// <summary>Gets the best move.</summary>
		protected override MoveDirection GetMove()
		{
			UpdateState();

			var hero = this.State.GetHero(this.Player);
			var source = this.Map[hero];

			Node.Lookup.Clear(this.State.Turn);
			var root = new RootNode(this.Map, this.State);
			root.GetMove(this.Map, this.Timout);

			return YgritteBot.BestMove;
		}

		/// <summary>The best move available.</summary>
		public static MoveDirection BestMove { get; set; }
	}
}
