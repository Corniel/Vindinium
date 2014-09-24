using System;
using System.Configuration;
using Vindinium.App;

namespace Vindinium.MonteCarlo
{
	public class MonteCarlo : Program<MonteCarlo> 
	{
		public MonteCarlo()
		{
			this.Simulation = new MonteCarloSimulation(17, true);
			this.MaxRuns = int.Parse(ConfigurationManager.AppSettings["maxruns"]);
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
		}
		public static void Main(string[] args)
		{
			MonteCarlo.DoMain(args);
		}

		/// <summary>Gets the map.</summary>
		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public MonteCarloSimulation Simulation { get; protected set; }
		public PlayerType Player { get; set; }

		public int MaxRuns { get; protected set; }
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

			Console.WriteLine(this.State.GetHero(this.Player).DebugToString());

			var move = this.Simulation.GetMove(this.Map, this.Player, this.State, this.Timout, this.Parameters.Turns, this.MaxRuns);
			Console.WriteLine("{0,4} Move: {1}, {2:#,##0.0}, {3:0.00}k, {4:0.0}s, {5:0.00}k/s",
				this.State.Turn,
				move, 
				this.Simulation.Score,
				this.Simulation.Simulations/1000.0,
				this.Simulation.Sw.Elapsed.TotalMilliseconds,
				this.Simulation.Simulations / this.Simulation.Sw.Elapsed.TotalMilliseconds);
			return move;
		}
	}
}
