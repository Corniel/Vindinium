using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vindinium.DrunkenViking.Strategies
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SafePathCollection: IEnumerable<SafePath>
	{
		public SafePathCollection()
		{
			this.SafePaths = new List<SafePath>();
			this.PotentialPaths = new Queue<PotentialPath>();
			this.Opponents = new List<PotentialOpponent>();
		}

		#region IEnumerable

		public int Count { get { return this.SafePaths.Count; } }

		public SafePath this[int index] { get { return this.SafePaths[index]; } }

		public IEnumerator<SafePath> GetEnumerator() { return this.SafePaths.GetEnumerator(); }

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() { return this.SafePaths.GetEnumerator(); }

		protected void Add(SafePath path)
		{
			if (path.Profit > 0)
			{
				this.SafePaths.Add(path);
			}
		}

		protected void Sort()
		{
			if (this.Count > 0)
			{
				var max = this.SafePaths.Max(path => path.Turns);
				var comparer = new SafePathComparer(max);
				this.SafePaths.Sort(comparer);
			}
		}

		#endregion

		protected List<SafePath> SafePaths {get;set;}
		protected Queue<PotentialPath> PotentialPaths { get; set; }
		protected List<PotentialOpponent> Opponents { get; set; }

		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public PlayerType PlayerToMove { get; protected set; }
		
		public SafePath BestPath { get { return this.SafePaths.Count == 0 ? null : this.SafePaths[0]; } }

		public MoveDirection BestMove { get { return this.SafePaths.Count == 0 ? MoveDirection.x : BestPath.Directions.ToArray()[0]; } }

		public Tile Source { get; protected set; }

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
			Sort();
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
			var health = potentialPath.Health;
			var turns = potentialPath.Turns;

			// If we are not healthy enough, quit.
			if (potentialPath.Health - (int)distance - Hero.HealthBattle < 1)
			{
				return;
			}

			

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
				health--;
				turns++;

				for (int option = 0; option < options; option++)
				{
					var tile = queue.Dequeue();

					foreach (var neighbor in tile.Neighbors)
					{
						if (ShouldEnqueue(distances, distancesToTarget, distance, turns, health, neighbor))
						{
							queue.Enqueue(neighbor);
						}
					}
				}
				foreach (var other in PlayerTypes.Other[this.PlayerToMove])
				{
					health--;
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
					this.Opponents.All(oppo => !oppo.StrongerAtLocationAndTime(this.Map, tile, health, turns));
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
						// we are already standing beside a mine.
						if (startDistance == Distance.Zero)
						{
							directions = source.Directions
								.Where(dir => source[dir].MineIndex == mineIndex)
								.ToArray();
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

				var followUp = PotentialPath.CreateFollowUp(target, health, mines, directions, turns, profit);
				this.PotentialPaths.Enqueue(followUp);
			}
		}
		protected void ProcessToTaverne(PotentialPath potentialPath, Tile target)
		{
			// We don't want to search for none profitable paths.
			if (potentialPath.Profit == 0 && this.SafePaths.Any(path => path.Profit > 0)) { return; }

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
						if (ShouldEnqueue(distances, distancesToTarget, distance, potentialPath.Turns, health, neighbor))
						{
							queue.Enqueue(neighbor);
						}
					}
				}
			}
			var isSafe = false;

			while (!isSafe && queue.Count > 0)
			{
				var tile = queue.Dequeue();
				isSafe = 
					health > Hero.HealthBattle || 
					this.Opponents.All(oppo => !oppo.StrongerAtLocationAndTime(this.Map, tile, health, turns));
			}
			if (isSafe)
			{
				var profit = potentialPath.Profit;
				profit += potentialPath.Mines.Count(this.PlayerToMove) * (int)startDistance;

				if (potentialPath.Directions == null)
				{
					var directions = source.Directions
						.Where(dir => distances.Get(source[dir]) < startDistance)
						.ToArray();

					// We just found a direct safe paht to a taverne.
					Add(new SafePath(potentialPath.Mines.Count(this.PlayerToMove), turns, profit, directions));
				}
				else
				{
					Add(potentialPath.ToSafePath(this.PlayerToMove, turns, profit));
				}
			}
		}

		private bool ShouldEnqueue(Distance[,] distances, Distance[,] distancesToTarget, Distance distance, int turns, int health, Tile target)
		{
			if (target.IsPassable && distances.Get(target) == Distance.Unknown)
			{
				var distanceToTarget = distancesToTarget.Get(target);
				distances.Set(target, distanceToTarget);
				if (distanceToTarget == distance)
				{
					return this.Opponents.All(opponent => !opponent.StrongerAtLocationAndTime(this.Map, target, health, turns));
				}
			}
			return false;
		}

		/// <summary>Returns an empty distance map.</summary>
		protected Distance[,] GetEmptyDistances()
		{
			return Distances.Create(this.Map);
		}

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return String.Format("Best: {0}, Count: {1}", this.BestPath == null ? "<none>" : this.BestPath.DebuggerDisplay, this.Count);
			}
		}

		public static SafePathCollection Create(Map map, State state)
		{
			var collection = new SafePathCollection();
			collection.Map = map;
			collection.State = state;
			collection.PlayerToMove = state.PlayerToMove;
			collection.Source = map[state.GetActiveHero()];
			collection.Opponents.AddRange(PotentialOpponent.Create(map, state));

			return collection;
		}
	}
}
