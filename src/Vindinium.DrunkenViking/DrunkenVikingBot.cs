using System;
using System.Configuration;
using Vindinium.App;

namespace Vindinium.DrunkenViking
{
	public class DrunkenVikingBot : Program<DrunkenVikingBot>
	{
		public static void Main(string[] args) { DrunkenVikingBot.DoMain(args); }

		public DrunkenVikingBot()
		{
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
		}

		/// <summary>Gets the map.</summary>
		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public PlayerType Player { get; set; }

		public TimeSpan Timout { get; protected set; }

		protected override void CreateGame()
		{
			this.Map = Map.Parse(this.Client.Response.game.board.ToRows());
			this.State = State.Create(this.Map);
			this.Player = this.Client.Response.hero.Player;
		}

		protected override MoveDirection GetMove()
		{
			this.State = this.State.Update(this.Client.Response.game);
			return MoveDirection.W;
		}
	}
}
