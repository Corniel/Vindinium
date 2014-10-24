using System;
using System.Collections.Generic;
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
		public TimeSpan Timout { get; protected set; }

		public override Move GetMove()
		{
			UpdateState();

			var collection = SafePathCollection.Create(this.Map, this.State);
			collection.Procces();
			
			var direction = collection.BestMove;
			var evaluation = collection.BestPath.Evaluation;

			return new Move(direction, evaluation);
		}
	}
}
