using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vindinium.App;
using Vindinium.Decisions;
using Vindinium.Ygritte2.Evaluation;

namespace Vindinium.Ygritte2.Decisions
{
	public class RootNode : Node
	{
		public RootNode(State state) : base(state, MoveDirection.x)
		{
			this.Stopwatch = new Stopwatch();
			this.Stopwatch.Start();
			this.Lookup = new NodeLookup();
		}

		public NodeLookup Lookup { get; protected set; }
		private Stopwatch Stopwatch { get; set; }

		/// <summary>Gets the best move.</summary>
		public MoveDirection BestMove
		{
			get { return this.Children == null ? MoveDirection.x : this.Children[0].Move; }
		}
		/// <summary>Gets the best score.</summary>
		public ScoreCollection BestScore { get { return this.Children[0].Score; } }

		public Move GetMove(ProcessData data, TimeSpan timeout)
		{
			var turn = this.Turn + 1;

			Run(data, timeout, turn);
			this.Stopwatch.Stop();
			Console.WriteLine();

			return new Move(this.BestMove, LogResult());
		}

		/// <summary>Runs the calculations.</summary>
		/// <remarks>
		/// Runs in a different thread.
		/// </remarks>
		private void Run(ProcessData data, TimeSpan timeout, int turn)
		{
			try
			{
				var source = new CancellationTokenSource((int)timeout.TotalMilliseconds);

				var task = Task.Factory.StartNew(() =>
				{
					while (turn < 1200)
					{
						this.Process(data, turn++, PotentialScore.EmptyCollection);
						LogResult();

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

			catch (Exception x)
			{
				Console.WriteLine(x);
#if DEBUG
				throw;
#endif
			}
		}
		private string LogResult()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("{0,4}. {1}", this.Turn, this.BestMove);

			var playerSearch = string.Format("h{0}", (int)this.PlayerToMove);
			var playerReplace = string.Format("h{0}*", (int)this.PlayerToMove);
			var score = this.Score.DebuggerDisplay.Replace("Score", "s").Replace(playerSearch, playerReplace);

			sb.AppendFormat(" = {0}", score);

			var ms = this.Stopwatch.Elapsed.TotalMilliseconds;

			sb.AppendFormat("  depth: {0}  {1:#,##0.0}ms", this.Lookup.Depth, ms);

			var nodes = this.Lookup.Nodes;
			var speed = nodes / ms;

			sb.AppendFormat("  {0:#,##0.0}k/s", speed);

			Console.Write("\r{0}", sb);
			return sb.ToString();
		}
	}
}
