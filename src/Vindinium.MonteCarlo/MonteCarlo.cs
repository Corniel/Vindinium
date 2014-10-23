using System;
using System.Configuration;
using Vindinium.App;
using Vindinium.Net;

namespace Vindinium.MonteCarlo
{
	public class MonteCarlo : Program<MonteCarlo> 
	{
		public MonteCarlo()
		{
			var runParallel = ConfigurationManager.AppSettings["run-parallel"] == "true";
			this.Simulation = new MonteCarloSimulation(17, runParallel);
			this.MaxRuns = int.Parse(ConfigurationManager.AppSettings["maxruns"]);
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
			this.Parameters = new ClientParameters();
		}
		public static void Main(string[] args)
		{
			Console.SetWindowSize(100, 20);
			MonteCarlo.DoMain(args);
		}

		public MonteCarloSimulation Simulation { get; protected set; }
		public int MaxRuns { get; protected set; }
		public TimeSpan Timout { get; protected set; }

		public override Move GetMove()
		{
			UpdateState();

			var direction = this.Simulation.GetMove(this.Map, this.Player, this.State, this.Timout, this.Parameters.Turns, this.MaxRuns);
			var evaluation = String.Format("{0,4} Move: {1}, {2:0.00}k, {3:0.0}s, {4:0.00}k/s",
				this.State.Turn,
				this.Simulation.Decision.ToConsoleDisplay(this.Player),
				this.Simulation.Simulations/1000.0,
				this.Simulation.Sw.Elapsed.TotalMilliseconds,
				this.Simulation.Simulations / this.Simulation.Sw.Elapsed.TotalMilliseconds);
			Console.WriteLine(evaluation);
			return new Move(direction, evaluation);
		}
	}
}
