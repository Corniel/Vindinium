using System;
using System.Collections.Generic;
using System.Linq;

namespace Vindinium.DrunkenViking.Strategies
{
	public class SafePathCollection
	{
		public SafePathCollection()
		{
			this.SafePaths = new List<SafePath>();
			this.PotentialPaths = new Queue<PotentialPath>();
		}
		protected List<SafePath> SafePaths {get;set;}
		protected Queue<PotentialPath> PotentialPaths { get; set; }

		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public PlayerType PlayerToMove { get; protected set; }
		public int Count { get { return this.SafePaths.Count; } }
		public SafePath BestPath { get { return this.SafePaths.Count == 0 ? null : this.SafePaths[0]; } }

		public MoveDirection BestMove { get { return this.SafePaths.Count == 0 ? MoveDirection.x : BestPath.Directions[0]; } }

		public Tile Source { get; protected set; }
		public Tile[] OtherHeros { get; protected set; }
		public Tile[] CrashedHeros { get; protected set; }

		public void Procces()
		{
			this.PotentialPaths.Enqueue(PotentialPath.Initial(this.Source, this.State));

			while (this.PotentialPaths.Count > 0)
			{
				var potentialPath = this.PotentialPaths.Dequeue();
				ProccesToMines(potentialPath);
				ProccesToTavernes(potentialPath);
			}

			// order them.
			this.SafePaths.Sort();
		}

		protected void ProccesToMines(PotentialPath potentialPath)
		{
			foreach (var mine in this.Map.Mines.Where(m => potentialPath.Mines[m.MineIndex] != this.PlayerToMove))
			{
				foreach (var target in mine.Neighbors.Where(n => n.IsPassable))
				{
					ProcessToMine(potentialPath, target, mine.MineIndex);
				}
			}
		}
		protected void ProccesToTavernes(PotentialPath potentialPath)
		{
			foreach (var taverne in this.Map.Tavernes)
			{
				foreach (var target in taverne.Neighbors.Where(n => n.IsPassable))
				{
					ProcessToTaverne(potentialPath, target);
				}
			}
		}

		protected void ProcessToMine(PotentialPath potentialPath, Tile target, int mineIndex)
		{
			var source = potentialPath.Source;

			var distances = GetEmptyDistances();
			var distancesToTarget = this.Map.GetDistances(target);

			var startDistance = distancesToTarget.Get(source);
			var distance = startDistance;

			// If we are not healthy enough, quit.
			if (potentialPath.Healths[this.PlayerToMove] - (int)distance - Hero.HealthBattle < 1)
			{
				return;
			}

			var healths = potentialPath.CloneHealths();

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
				healths[this.PlayerToMove]--;

				for (int option = 0; option < options; option++)
				{
					var tile = queue.Dequeue();

					foreach (var neighbor in tile.Neighbors)
					{
						if (ShouldEnqueue(distances, distancesToTarget, distance, potentialPath.Turns, neighbor))
						{
							queue.Enqueue(neighbor);
						}
					}
				}
				foreach (var other in PlayerTypes.Other[this.PlayerToMove])
				{
					healths[other]--;
				}
			}

			var isSafe = false;
			while (!isSafe && queue.Count > 0)
			{
				isSafe = IsSafe(Distance.Zero, potentialPath.Turns, queue.Dequeue());
			}
			if (isSafe)
			{
				var directions = potentialPath.Directions;
				if (directions == null)
				{
					directions = source.Directions
						.Where(dir => distances.Get(source[dir]) < startDistance)
						.ToArray();

					if (directions.Length == 0)
					{
#if DEBUG
						throw new Exception("No Directions found.");
#else
						return;
#endif
					}
				}
				var mines = potentialPath.Mines.Set(mineIndex, this.PlayerToMove);
				healths[this.PlayerToMove] -= Hero.HealthBattle;

				var turns = potentialPath.Turns + (int)startDistance + 1;
				var profit = potentialPath.Profit;
				profit += (mines.Count(this.PlayerToMove) - 1) * ((int)startDistance - 1) + 1;

				var followUp = PotentialPath.CreateFollowUp(target, healths, mines, directions, turns, profit);
				this.PotentialPaths.Enqueue(followUp);
			}
		}

		protected void ProcessToTaverne(PotentialPath potentialPath, Tile target)
		{
			// We don't want to searh for none profitable paths.
			if (potentialPath.Profit == 0 && this.SafePaths.Any(path => path.Profit > 0)) { return; }

			var source = potentialPath.Source;
			var distances = GetEmptyDistances();
			var distancesToTarget = this.Map.GetDistances(target);

			var startDistance = distancesToTarget.Get(source);
			var distance = startDistance;

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

				for (int option = 0; option < options; option++)
				{
					var tile = queue.Dequeue();
					foreach (var neighbor in tile.Neighbors)
					{
						if (ShouldEnqueue(distances, distancesToTarget, distance, potentialPath.Turns, neighbor))
						{
							queue.Enqueue(neighbor);
						}
					}
				}
				var isSafe = false;
				while (!isSafe && queue.Count > 0)
				{
					isSafe = IsSafe(Distance.Zero, potentialPath.Turns, queue.Dequeue());
				}
				if (isSafe)
				{
					var turns = potentialPath.Turns + (int)startDistance;
					var profit = potentialPath.Profit;
					profit += potentialPath.Mines.Count(this.PlayerToMove) * (int)startDistance;

					this.SafePaths.Add(potentialPath.ToSafePath(turns, profit));
				}
			}
		}

		private bool ShouldEnqueue(Distance[,] distances, Distance[,] distancesToTarget, Distance distance, int turns, Tile neighbor)
		{
			if (neighbor.IsPassable && distances.Get(neighbor) == Distance.Unknown)
			{
				var distanceToTarget = distancesToTarget.Get(neighbor);
				distances.Set(neighbor, distanceToTarget);
				if (distanceToTarget == distance && IsSafe(distance, turns, neighbor))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsSafe(Distance distance, int turns, Tile neighbor)
		{
			// if we reach the mine, it takes a turn to take it.
			var safeDistance = turns + (int)distance + 1 + (distance == Distance.Zero ? 1 : 0);

			if (safeDistance > 0)
			{
				if (this.OtherHeros.All(other => this.Map.GetDistances(other).Get(neighbor) > safeDistance))
				{
					if (this.CrashedHeros.All(other => this.Map.GetDistances(other).Get(neighbor) > Distance.One))
					{
						return true;
					}
				}
			}
			return false;
		}


		/// <summary>Returns an empty distance map.</summary>
		protected Distance[,] GetEmptyDistances()
		{
			return Distances.Create(this.Map);
		}

		public static SafePathCollection Create(Map map, State state)
		{
			var collection = new SafePathCollection();
			collection.Map = map;
			collection.State = state;
			collection.PlayerToMove = state.PlayerToMove;
			collection.Source = map[state.GetActiveHero()];
			collection.OtherHeros = PlayerTypes.Other[collection.PlayerToMove].Where(p => !state.GetHero(p).IsCrashed).Select(p => map[state.GetHero(p)]).ToArray();
			collection.CrashedHeros = PlayerTypes.Other[collection.PlayerToMove].Where(p => state.GetHero(p).IsCrashed).Select(p => map[state.GetHero(p)]).ToArray();
			
			return collection;
		}
	}

	
}
