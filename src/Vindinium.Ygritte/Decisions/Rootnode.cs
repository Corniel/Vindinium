using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vindinium.Decisions;

namespace Vindinium.Ygritte.Decisions
{
	public class RootNode : Node
	{
		public RootNode(Map map, State state) : base(map, state)
		{
			this.MoveMappings = new Dictionary<Node, MoveDirection>();
			this.Stopwatch = new Stopwatch();
			this.Stopwatch.Start();
		}

		private Dictionary<Node, MoveDirection> MoveMappings { get; set; }

		private Stopwatch Stopwatch { get; set; }

		/// <summary>Gets the best move.</summary>
		public MoveDirection BestMove { get { return this.MoveMappings[this.Children[0]]; } }
		/// <summary>Gets the best score.</summary>
		public ScoreCollection BestScore { get { return this.Children[0].Score; } }
		/// <summary>Gets the scores.</summary>
		public List<Tuple<MoveDirection, ScoreCollection>> Scores
		{
			get
			{
				return this.Children.Select(ch => new Tuple<MoveDirection, ScoreCollection>(MoveMappings[ch], ch.Score)).ToList();
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

		public void GetMove(Map map, TimeSpan timeout)
		{
			InitializeMoveMappings(map);

			var turn = this.Turn + 1;

			Run(map, timeout, turn);
			this.Stopwatch.Stop();
			LogResult();
			Console.WriteLine();
		}

		/// <summary>Runs the calculations.</summary>
		/// <remarks>
		/// Runs in a different thread.
		/// </remarks>
		private void Run(Map map, TimeSpan timeout, int turn)
		{
			try
			{
				var source = new CancellationTokenSource((int)timeout.TotalMilliseconds);

				var task = Task.Factory.StartNew(() =>
				{
					while (true)
					{
						this.Process(map, turn++, PotentialScore.EmptyCollection);
						YgritteBot.BestMove = this.BestMove;
						LogResult();
						turn++;
						// Was cancellation already requested?  
						if (source.IsCancellationRequested == true)
						{
							source.Token.ThrowIfCancellationRequested();
						}
					}
				}, source.Token);
				if (task.Wait((int)timeout.TotalMilliseconds))
				{
					source.Cancel();
				}
			}
			catch (OperationCanceledException) { }
			catch (AggregateException) { }
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
