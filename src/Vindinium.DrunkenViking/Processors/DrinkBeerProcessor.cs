using System;
using System.Linq;

namespace Vindinium.DrunkenViking.Processors
{
	/// <summary>Our hero stands beside a Taverne. To drink or not to drink, that's the question.</summary>
	public class DrinkBeerProcessor : Processor
	{
		/// <summary>Processes the potential path.</summary>
		public override void Process(PotentialPath potentialPath, SafePathCollection collection)
		{
			var source = potentialPath.Source;
			if (source.Neighbors.Any(n => n.IsTaverne))
			{
				var directions = new MoveDirections(source.Directions.Where(direction => source[direction].IsTaverne));

				var turns = 0;
				var health = potentialPath.Health;
				var profit = 0;
				var mines = potentialPath.Mines.Count(this.PlayerToMove);

				while (health < Hero.HealthMax)
				{
					turns++;
					health = Math.Min(health + Hero.HealthTavern, Hero.HealthMax);
					profit += mines - Hero.CostsTavern;

					var followUp = potentialPath.CreateFollowUp(
						source, 
						health - 1,
						potentialPath.Mines,
						directions, 
						turns,
						profit);

					collection.Enqueue(followUp);
				}
			}
		}
	}
}
