using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Vindinium.DrunkenViking.Processors;

namespace Vindinium.DrunkenViking
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SafePathCollection: IEnumerable<SafePath>
	{
		public SafePathCollection()
		{
			this.SafePaths = new List<SafePath>();
			this.PotentialPaths = new Queue<PotentialPath>();
		}

		#region IEnumerable
		
		protected List<SafePath> SafePaths { get; set; }

		public int Count { get { return this.SafePaths.Count; } }

		public SafePath this[int index] { get { return this.SafePaths[index]; } }

		public IEnumerator<SafePath> GetEnumerator() { return this.SafePaths.GetEnumerator(); }

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() { return this.SafePaths.GetEnumerator(); }

		public void Add(SafePath path)
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

		public void Enqueue(PotentialPath path)
		{
			this.PotentialPaths.Enqueue(path);
		}
		public List<PotentialPath> GetPotentialPaths()
		{
			return this.PotentialPaths.ToList();
		}

		protected Queue<PotentialPath> PotentialPaths { get; set; }

		public Map Map { get; protected set; }
		public State State { get; protected set; }
		public SafePath BestPath { get { return this.SafePaths.Count == 0 ? null : this.SafePaths[0]; } }
		public MoveDirection BestMove { get { return this.SafePaths.Count == 0 ? MoveDirection.x : BestPath.Directions.ToArray()[0]; } }

		public Tile Source { get; protected set; }

		public void Procces()
		{
			Add(SafePath.NoPath);

			var processors = new List<Processor>()
			{
				new DrinkBeerProcessor(),
				new ToMineProcessor(),
				new ToTavernPorcessor(),
			};
			foreach (var processor in processors)
			{
				processor.Initialize(this.Map, this.State);
			}
			Enqueue(PotentialPath.Initial(this.Source, this.Map, this.State));

			while (this.PotentialPaths.Count > 0)
			{
				var potentialPath = this.PotentialPaths.Dequeue();

				foreach (var processor in processors)
				{
					processor.Process(potentialPath, this);
				}
			}
			// order them.
			Sort();
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
			collection.Source = map[state.GetActiveHero()];

			return collection;
		}
	}
}
