using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.DrunkenViking;

namespace Vindinium.DrunkenViking.Processors
{
	public class ToMineProcessor : Processor
	{
		public override void Process(PotentialPath potentialPath, SafePathCollection collection)
		{
			foreach (var mine in this.Map.Mines.Where(m => potentialPath.Mines[m.MineIndex] != this.PlayerToMove))
			{
				foreach (var target in mine.Neighbors.Where(n => n.IsPassable))
				{
					ProcessToMine(potentialPath, target, mine.MineIndex, collection);
				}
			}
		}

		public void ProcessToMine(PotentialPath potentialPath, Tile target, int mineIndex, SafePathCollection collection)
		{
			var source = potentialPath.Source;

			var distances = GetEmptyDistances();
			var distancesToTarget = this.Map.GetDistances(target);

			var startDistance = distancesToTarget.Get(source);
			var distance = startDistance;
			var health = potentialPath.Health;
			var turns = potentialPath.Turns;

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
						//if (ShouldEnqueue(distances, distancesToTarget, distance, turns, health, neighbor))
						//{
						//	queue.Enqueue(neighbor);
						//}
					}
				}
			}

			var isSafe = false;
			// It takes another turn to get the mine.
			turns++;

			while (!isSafe && queue.Count > 0)
			{
				var tile = queue.Dequeue();
				isSafe =
					health > Hero.HealthBattle ||
					potentialPath.Opponents.All(oppo => !oppo.StrongerAtLocationAndTime(this.Map, tile, health, turns));
			}
			if (isSafe)
			{
				var directions = potentialPath.Directions;
				if (directions.IsEmpty())
				{
					directions = new MoveDirections(source.Directions.Where(dir => distances.Get(source[dir]) < startDistance));

					if (directions.Length == 0)
					{
						// we are already standing beside a mine.
						if (startDistance == Distance.Zero)
						{
							directions = new MoveDirections(source.Directions.Where(dir => source[dir].MineIndex == mineIndex));
						}
						if (directions.Length == 0)
						{
#if DEBUG
							throw new Exception("No directions found.");
#else
						return;
#endif
						}
					}
				}
				var mines = potentialPath.Mines.Set(mineIndex, this.PlayerToMove);
				health -= Hero.HealthBattle;

				var profit = potentialPath.Profit;
				profit += (mines.Count(this.PlayerToMove) - 1) * (turns - potentialPath.Turns) + 1;

				var followUp = potentialPath.CreateFollowUp(target, health, mines, directions, turns, profit);
				collection.Enqueue(followUp);
			}
		}
	}
}
