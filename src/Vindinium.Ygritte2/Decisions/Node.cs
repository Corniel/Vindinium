using System.Collections.Generic;
using Vindinium.Decisions;

namespace Vindinium.Ygritte2.Decisions
{
	public class Node
	{
		public Node(State state, MoveDirection dir)
		{
			this.State = state;
			this.Move = dir;
		}

		public ScoreCollection Score { get; protected set; }
		public State State { get; protected set; }
		public List<Node> Children { get; protected set; }
		public MoveDirection Move { get; protected set; }

		public int Turn { get { return this.State.Turn; } }
		public PlayerType PlayerToMove { get { return this.State.PlayerToMove; } }

		public ScoreCollection Process(ProcessData data, int turn, ScoreCollection alphas)
		{
			var map = data.Map;
			var resultAlphas = PotentialScore.EmptyCollection;

			if (this.Turn >= 1199 || turn <= this.Turn)
			{
				this.Score = data.Evalutor.Evaluate(this.State);
				alphas.ContinueProccesingAlphas(this.Score, this.PlayerToMove, out resultAlphas);
				return resultAlphas;
			}

			if (this.Children == null)
			{
				this.Children = new List<Node>();

				foreach (var dir in data.MoveGenerator.Generate(map, this.State))
				{
					var state = this.State.Move(map, dir, this.PlayerToMove);
					var child = data.Lookup.Get(this.Turn + 1, state, dir);
					this.Children.Add(child);
				}
			}

			var test = PotentialScore.EmptyCollection;

			for (int i = 0; i < this.Children.Count; i++)
			{
				var child = this.Children[i];
				switch (i)
				{
					case 0: test = child.Process(data, turn - 0, alphas); break;
					case 1: test = child.Process(data, turn - 1, alphas); break;
					case 2: test = child.Process(data, turn - 2, alphas); break;
					case 3: test = child.Process(data, turn - 3, alphas); break;
					default:
					case 4: test = child.Process(data, turn - 5, alphas); break;
				}

				if (!alphas.ContinueProccesingAlphas(test, this.PlayerToMove, out resultAlphas))
				{
					break;
				}
			}
			var comparer = NodeComparer.Get(this.PlayerToMove);
			Children.Sort(comparer);
			this.Score = this.Children[0].Score;

			return resultAlphas;
		}
	}
}
