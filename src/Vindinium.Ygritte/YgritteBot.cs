using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.App;
using Vindinium.Ygritte.Decisions;

namespace Vindinium.Ygritte
{
	public class YgritteBot : Program<YgritteBot>
	{
		public static void Main(string[] args) { YgritteBot.DoMain(args); }

		public YgritteBot()
		{
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
		}

		/// <summary>Gets the map.</summary>
		public Map Map { get; protected set; }
		public PlayerType Player { get; set; }

		public TimeSpan Timout { get; protected set; }

		protected override void CreateGame()
		{
			this.Map = Map.Parse(this.Client.Response.game.board.ToRows());
			this.State = State.Create(this.Map);
			this.Player = this.Client.Response.hero.Player;

			Node.Lookup.Clear();
		}

		protected override MoveDirection GetMove()
		{
			this.State = this.State.Update(this.Client.Response.game);
			this.Map.Update(this.Client.Response.game);

			var hero = this.State.GetHero(this.Player);
			var source = this.Map[hero];

			Node.Lookup.Clear(this.State.Turn);
			var root = new RootNode(this.Map, this.State);
			var md = root.GetMove(this.Map, this.Timout);
			return md;
		}
	}
}
