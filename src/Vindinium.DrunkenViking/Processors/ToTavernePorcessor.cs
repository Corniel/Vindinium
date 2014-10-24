using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Processors
{
	public class ToTavernePorcessor : Processor
	{
		public override void Process(PotentialPath potentialPath, SafePathCollection collection)
		{
			foreach (var taverne in this.Map.Tavernes)
			{
				foreach (var target in taverne.Neighbors.Where(n => n.IsPassable))
				{
					ProcessToTaverne(potentialPath, target, collection);
				}
			}
		}

		public void ProcessToTaverne(PotentialPath potentialPath, Tile target, SafePathCollection collection)
		{
			var source = potentialPath.Source;
			var distances = GetEmptyDistances();
			var distancesToTarget = this.Map.GetDistances(target);

			var startDistance = distancesToTarget.Get(source);
			var distance = startDistance;
			var health = potentialPath.Health;
			var turns = potentialPath.Turns;

			// We don't consider it a safe path (yet) when we are already beside a Taverne.
			if (potentialPath.Turns == 0 && startDistance == Distance.Zero) { return; }

			var queue = new Queue<Tile>();
			distances.Set(source, distance);
			queue.Enqueue(source);

			while (distance > Distance.Zero)
			{
				var options = queue.Count;

				// No options available.
				if (options == 0)
				{
					return;
				}
				distance--;
				health = Math.Max(Hero.HealthMin, health - 1);
				turns++;

				for (int option = 0; option < options; option++)
				{
					var tile = queue.Dequeue();
					foreach (var neighbor in tile.Neighbors)
					{
						//if (ShouldEnqueue(distances, distancesToTarget, distance, potentialPath.Turns, health, neighbor))
						//{
						//	queue.Enqueue(neighbor);
						//}
					}
				}
			}
			var isSafe = false;

			while (!isSafe && queue.Count > 0)
			{
				var tile = queue.Dequeue();
				isSafe =
					health > Hero.HealthBattle ||
					potentialPath.Opponents.All(oppo => !oppo.StrongerAtLocationAndTime(this.Map, tile, health, turns));
			}
			if (isSafe)
			{
				var profit = potentialPath.Profit;
				profit += potentialPath.Mines.Count(this.PlayerToMove) * (int)startDistance;

				if (potentialPath.Directions.IsEmpty())
				{
					var directions = source.Directions
						.Where(dir => distances.Get(source[dir]) < startDistance)
						.ToArray();

					// We just found a direct safe paht to a taverne.
					collection.Add(new SafePath(potentialPath.Mines.Count(this.PlayerToMove), turns, profit, directions));
				}
				else
				{
					collection.Add(potentialPath.ToSafePath(this.PlayerToMove, turns, profit));
				}
			}
		}
	}
}
