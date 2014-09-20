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

		public MoveDirection GetMove(Map map, State state, TimeSpan timeout)
		{
			var sw = new Stopwatch();
			sw.Start();

			var player = state.PlayerToMove;
			var hero = state.GetHero(player);
			var source = map[hero];

			options.Clear();
			foreach (var dir in source.Directions)
			{
				options[dir] = 0;
			}
			runs = 0;

			while (sw.Elapsed < timeout)
			{
				if (RunParallel)
				{
					Parallel.ForEach(source.Directions, (dir) =>
					{
						var rnd = Rnds[dir];
						var target = source[dir];
						GetScore(map, state, hero, player, source, target, dir, rnd);
					});
				}
				else
				{
					foreach (var dir in source.Directions)
					{
						var target = source[dir];
						GetScore(map, state, hero, player, source, target, dir, this.Rnds[MoveDirection.N]);
					}
				}
			}
			return options.OrderByDescending(kvp => kvp.Value).First().Key;
		}

		private void GetScore(Map map, State state, Hero hero, PlayerType player, Tile source, Tile target, MoveDirection move, MT19937Generator rnd)
		{
			var score = Score(map, state, hero, player, source, target, player, rnd);
			lock (locker)
			{
				options[move] += score;
				runs++;
			}
		}
		private Dictionary<MoveDirection, long> options = new Dictionary<MoveDirection, long>();
		private int runs = 0;

		public int Simulations { get { return runs; } }

		private int Score(Map map, State old_state, Hero hero, PlayerType player, Tile source, Tile target, PlayerType playerToSimulate, MT19937Generator rnd)
		{
			var new_state = old_state.Move(map, hero, player, source, target);

			if (new_state.Turn >= 1200)
			{
				return new_state.GetHero(playerToSimulate).Gold;
			}

			player = new_state.PlayerToMove;
			hero = new_state.GetHero(player);
			source = map[hero];
			target = source.Neighbors[rnd.Next(source.Neighbors.Length)];

			return Score(map, new_state, hero, player, source, target, playerToSimulate, rnd);
		}
	}
}
