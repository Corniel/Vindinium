using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;
using Vindinium.Decisions;
using Vindinium.MonteCarlo.Decisions;

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
			};
		}
		public bool RunParallel { get; protected set; }
		public Dictionary<MoveDirection, MT19937Generator> Rnds { get; protected set; }

		public Stopwatch Sw { get; protected set; }

		public PlayerType PlayerToMove { get; protected set; }

		public MoveDirection GetMove(Map map, PlayerType playerToMove, State state, TimeSpan timeout, int turns, int maxruns = int.MaxValue)
		{
			this.Sw = new Stopwatch();
			this.Sw.Start();
			this.PlayerToMove = playerToMove;

			var hero = state.GetHero(playerToMove);
			var source = map[hero];

			options.Clear();
			foreach (var dir in source.Directions.Where(d => d != MoveDirection.x))
			{
				options[dir] = new Dictionary<PlayerType, long>()
				{
					{ PlayerType.Hero1, 0 },
					{ PlayerType.Hero2, 0 },
					{ PlayerType.Hero3, 0 },
					{ PlayerType.Hero4, 0 },
				};
			}
			runs = 0;

			while (runs < maxruns && this.Sw.Elapsed < timeout)
			{
				if (RunParallel)
				{
					Parallel.ForEach(source.Directions.Where(d => d != MoveDirection.x), (dir) =>
					{
						var rnd = Rnds[dir];
						var target = source[dir];
						GetScore(map, state, hero, playerToMove, source, target, dir, rnd, turns);
					});
				}
				else
				{
					foreach (var dir in source.Directions.Where(d => d != MoveDirection.x))
					{
						var target = source[dir];
						GetScore(map, state, hero, playerToMove, source, target, dir, this.Rnds[MoveDirection.N], turns);
					}
				}
			}
			this.Sw.Stop();

			this.Decision = AverageScoreDecision.Create(options, runs, this.PlayerToMove);
			return this.Decision.Move;
		}

		private void GetScore(Map map, State state, Hero hero, PlayerType player, Tile source, Tile target, MoveDirection move, MT19937Generator rnd, int turns)
		{
			var score = GetScore(map, state, hero, player, source, target, player, rnd, turns);
			lock (locker)
			{
				options[move][PlayerType.Hero1] += score.Hero1;
				options[move][PlayerType.Hero2] += score.Hero2;
				options[move][PlayerType.Hero3] += score.Hero3;
				options[move][PlayerType.Hero4] += score.Hero4;
				runs++;
			}
		}
		private Dictionary<MoveDirection, Dictionary<PlayerType, long>> options = new Dictionary<MoveDirection, Dictionary<PlayerType, long>>();
		private int runs = 0;

		/// <summary>Gets the score.</summary>
		public AverageScoreDecision Decision { get; protected set; }
		
		public int Simulations { get { return runs; } }

		private ScoreCollection GetScore(Map map, State old_state, Hero hero, PlayerType playerToMove, Tile source, Tile target, PlayerType playerToSimulate, MT19937Generator rnd, int turns)
		{
			var new_state = old_state.Move(map, hero, playerToMove, source, target);

			if (new_state.Turn >= turns)
			{
				var score = CurrentStateEvaluator.Instance.Evaluate(new_state, map);
				return score;
			}

			playerToMove = PlayerTypes.Next(playerToMove);
			hero = new_state.GetHero(playerToMove);
			source = map[hero];
			target = hero.IsCrashed ? source : source.Targets[rnd.Next(source.Targets.Length)];

			return GetScore(map, new_state, hero, playerToMove, source, target, playerToSimulate, rnd, turns);
		}
	}
}
