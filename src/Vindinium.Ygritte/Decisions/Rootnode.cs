using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	public class RootNode : Node
	{
		public RootNode(State state) : base(state)
		{
			this.MoveMappings = new Dictionary<Node, MoveDirection>();
			this.Stopwatch = new Stopwatch();
			this.Stopwatch.Start();
		}

		private Dictionary<Node, MoveDirection> MoveMappings { get; set; }

		private Stopwatch Stopwatch { get; set; }

		/// <summary>Gets the best move.</summary>
		public MoveDirection BestMove
		{
			get
			{
				return this.MoveMappings[this.Children[0]];
			}
		}

		public void InitializeMoveMappings(Map map)
		{
			var hero = this.State.GetActiveHero();
			var source = map[hero];

			foreach (var dir in source.Directions)
			{
				var target = source[dir];
				var nw_sta = this.State.Move(map, hero, this.PlayerToMove, source, target);
				var child = Node.Get(this.Turn + 1, map, nw_sta);
				child.ClearMoves();
				this.MoveMappings[child] = dir;
			}
		}

		public MoveDirection GetMove(Map map, TimeSpan timeout)
		{
			InitializeMoveMappings(map);

			var turn = this.Turn + 1;

			Run(map, timeout, turn);
			// RunAsync(map, timeSpan, turn);

			this.Stopwatch.Stop();
			LogResult();
			Console.WriteLine();

			return this.BestMove;
		}

		
		private void Run(Map map, TimeSpan timeout, int turn)
		{
			while (Stopwatch.Elapsed < timeout)
			{
				this.Process(map, turn++, Score.MinScore);
				LogResult();
				turn++;
			}
		}

		private void RunAsync(Map map, TimeSpan timeout, int turn)
		{
			var run = RunProcces(map, turn);

			if (Task.WhenAny(run, Task.Delay((int)timeout.TotalMilliseconds)) == run)
			{
			}
			Thread.Sleep((int)timeout.TotalMilliseconds);
		}

		private async Task RunProcces(Map map, int turn)
		{
			var result = Task.Run(() =>
			{
				while (true)
				{
					this.Process(map, turn++, Score.MinScore);

					if (this.Children.Count == 0)
					{
					}
					LogResult();
					turn++;
				}
			});
			await result;
		}

		private void LogResult()
		{
			var playerstr = string.Format("[{0}]", (int)this.PlayerToMove);

			Console.Write("\r[{0,4}] {1,4}, {4}, d: {2}, {3}",
							this.Turn,
							this.Stopwatch.ElapsedMilliseconds,
							Node.Lookup.Depth, 
							this.Score.DebuggerDisplay
								.Replace("Score", "s")
								.Replace(playerstr, "[*]"),
							this.MoveMappings[this.Children[0]]);
		}
	}
}
