using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace Vindinium
{
	public class MonteCarloSimulation
	{
		public MonteCarloSimulation() { }

		private static volatile object locker = new object();

		public MonteCarloSimulation(int seed, bool runParallel = true)
		{
			this.RunParallel = runParallel;
			this.Rnds = new Dictionary<MoveDirection, MT19937Generator>()
			{
				{ MoveDirection.N, new MT19937Generator(seed + 1) },
				{ MoveDirection.W, new MT19937Generator(seed + 2) },
				{ MoveDirection.S, new MT19937Generator(seed + 3) },
				{ MoveDirection.E, new MT19937Generator(seed + 4) },
				{ MoveDirection.x, new MT19937Generator(seed + 5) },
			};
		}
		public bool RunParallel { get; protected set; }
		public Dictionary<MoveDirection, MT19937Generator> Rnds { get; protected set; }

		public Stopwatch Sw { get; protected set; }

		public MoveDirection GetMove(Map map, PlayerType playerToMove, State state, TimeSpan timeout, int turns, int maxruns = int.MaxValue)
		{
			this.Sw = new Stopwatch();
			this.Sw.Start();

			turns <<= 2;

			var hero = state.GetHero(playerToMove);
			var source = map[hero];

			options.Clear();
			foreach (var dir in source.Directions)
			{
				options[dir] = 0;
			}
			runs = 0;

			while (runs < maxruns && this.Sw.Elapsed < timeout)
			{
				if (RunParallel)
				{
					Parallel.ForEach(source.Directions, (dir) =>
					{
						var rnd = Rnds[dir];
						var target = source[dir];
						GetScore(map, state, hero, playerToMove, source, target, dir, rnd, turns);
					});
				}
				else
				{
					foreach (var dir in source.Directions)
					{
						var target = source[dir];
						GetScore(map, state, hero, playerToMove, source, target, dir, this.Rnds[MoveDirection.N], turns);
					}
				}
			}
			this.Sw.Stop();
			return options.OrderByDescending(kvp => kvp.Value).First().Key;
		}

		private void GetScore(Map map, State state, Hero hero, PlayerType player, Tile source, Tile target, MoveDirection move, MT19937Generator rnd, int turns)
		{
			var score = GetScore(map, state, hero, player, source, target, player, rnd, turns);
			lock (locker)
			{
				options[move] += score;
				runs++;
			}
		}
		private Dictionary<MoveDirection, long> options = new Dictionary<MoveDirection, long>();
		private int runs = 0;

		/// <summary>Gets the score.</summary>
		public double Score
		{
			get
			{
				return 1.0 * options.Count * options.Values.Max() / runs;
			}
		}

		public int Simulations { get { return runs; } }

		private int GetScore(Map map, State old_state, Hero hero, PlayerType playerToMove, Tile source, Tile target, PlayerType playerToSimulate, MT19937Generator rnd, int turns)
		{
			var new_state = old_state.Move(map, hero, playerToMove, source, target);

			if (new_state.Turn >= turns)
			{
				var score = new_state.GetHero(playerToSimulate).Gold;
				//foreach (var other in PlayerTypes.Other[playerToSimulate])
				//{
				//	score -= new_state.GetHero(other).Gold;
				//}
				return score;
			}

			playerToMove = PlayerTypes.Next[playerToMove];
			hero = new_state.GetHero(playerToMove);
			source = map[hero];
			target = source.Neighbors[rnd.Next(source.Neighbors.Length)];

			return GetScore(map, new_state, hero, playerToMove, source, target, playerToSimulate, rnd, turns);
		}
	}
}
