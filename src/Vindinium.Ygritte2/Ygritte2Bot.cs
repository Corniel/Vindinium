using System;
using System.Configuration;
using Vindinium.App;
using Vindinium.Ygritte2.Decisions;
using Vindinium.Ygritte2.Evaluation;

namespace Vindinium.Ygritte2
{
	public class Ygritte2Bot : Program<Ygritte2Bot>
	{
		/// <summary>The main entry point of the program.</summary>
		public static void Main(string[] args)
		{
			Console.SetWindowSize(120, 40);
			Ygritte2Bot.DoMain(args); 
		}

		/// <summary>Constructor.</summary>
		public Ygritte2Bot()
		{
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
		}

		/// <summary>Gets the time-out for the bot.</summary>
		public TimeSpan Timout { get; protected set; }
		public Evaluator Evaluator { get; protected set; }

		protected override void CreateGame()
		{
			base.CreateGame();

			this.Evaluator = Evaluator.Get(this.Map);
		}


		public override Move GetMove()
		{
			UpdateState();

			var node = new RootNode(this.State);
			var data = new ProcessData(node.Lookup, this.Map, this.Evaluator);
			var move = node.GetMove(data, this.Timout);
			return move;
		}
	}
}
