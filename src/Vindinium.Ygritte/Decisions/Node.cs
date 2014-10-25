using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.Decisions;

namespace Vindinium.Ygritte.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class Node
	{
		public const int CombatDistance = 3;

		public Node(Map map, State state)
		{
			this.State = state;
			this.Score = YgritteStateEvaluator.Instance.Evaluate(map, state);

			this.Children = new List<Node>();
			this.Moves = new Dictionary<PlayerType, List<Move>>()
			{
				{ PlayerType.Hero1, new List<Move>() },
				{ PlayerType.Hero2, new List<Move>() },
				{ PlayerType.Hero3, new List<Move>() },
				{ PlayerType.Hero4, new List<Move>() },
			};
		}
		public static readonly NodeLookup Lookup = new NodeLookup();

		public static Node Get(int turn, Map map, State state)
		{
			return Lookup.Get(turn, map, state);
		}

		public ScoreCollection Score { get; protected set; }
		public State State { get; protected set; }
		public int Turn { get { return this.State.Turn; } }
		public PlayerType PlayerToMove { get { return this.State.PlayerToMove; } }
		
		public Dictionary<PlayerType, List<Move>> Moves { get; protected set; }

		public List<Node> Children { get; set; }

		public ScoreCollection Process(Map map, int turn, ScoreCollection alphas)
		{
			var resultAlphas = PotentialScore.EmptyCollection;

			if (this.Turn >= 1199 || turn <= this.Turn)
			{
				alphas.ContinueProccesingAlphas(this.Score, this.PlayerToMove, out resultAlphas);
				return resultAlphas; 
			}

			var moves = this.Moves[this.PlayerToMove];

			if (this.Children.Count == 0 || moves.Count == 0)
			{
				// Reset is Cleared, so after us, others can add moves if needed.
				this.IsCleared = false;
				var player = this.PlayerToMove;
				var hero = this.State.GetHero(player);
				var source = map[hero];

				var health = hero.Health;
				var mines = hero.Mines;

				if (moves.Count == 0)
				{
					var plans = GeneratePlans(map, this.State, hero, player);
					foreach (var plan in plans)
					{
						MovesGenerator.Instance.AddMoves(moves, map, this.State, source, hero, player, plan);
					}
				}
				foreach (var move in moves)
				{
					var target = move.GetTarget(source, map, this.State);
					if (target != null)
					{
						var state = this.State.Move(map, hero, player, source, target);
						var child = Node.Get(this.Turn + 1, map, state);

						var heroNew = state.GetHero(player);

						// if no tarverne hit, if no mine changes, and no enermies as neigbhor.
						if (child.IsCleared) { }
						else if (heroNew.Health <= health &&
							heroNew.Mines == mines &&
							PlayerTypes.Other[player].All(p => Map.GetManhattanDistance(heroNew, state.GetHero(p)) > 2))
						{
							child.AddMove(move, player);
						}
						else
						{
							child.ClearMoves();
						}
						if (!this.Children.Contains(child))
						{
							this.Children.Add(child);
						}
					}
				}
			}

			var test = PotentialScore.EmptyCollection;

			for (int i = 0; i < this.Children.Count; i++)
			{
				var child = this.Children[i];
				switch (i)
				{
					case 0:
					case 1: test = child.Process(map, turn, alphas); break;
					case 2:
					case 3: test = child.Process(map, turn - 1, alphas); break;
					default:
					case 4: test = child.Process(map, turn - 2, alphas); break;
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

		public void AddMove(Move move, PlayerType player)
		{
			var moves = this.Moves[player];

			if (!moves.Contains(move))
			{
				moves.Add(move);
			}
		}
		public void ClearMoves()
		{
			this.IsCleared = true;
			foreach (var player in PlayerTypes.All)
			{
				this.Moves[player].Clear();
			}
		}
		public bool IsCleared { get; protected set; }

		public List<PlanType> GeneratePlans(Map map, State state, Hero hero, PlayerType player)
		{
			var plans = new List<PlanType>();

			if (hero.IsCrashed)
			{
				plans.Add(PlanType.Crashed);
			}
			else
			{
				var source = map[hero];
				int health = hero.Health;
				int mines = hero.Mines;


				foreach(var other in PlayerTypes.Other[player].Select(other => state.GetHero(other)))
				{
					// some what close.
					if (Map.GetManhattanDistance(hero, other) <= Node.CombatDistance)
					{
						plans.Add(PlanType.Combat);
					}
				}

				// We are winning, keep the position.
				if (mines >= (map.Mines.Length >> 1))
				{
					plans.Add(PlanType.ToTavern);

					if (map.GetDistanceToTavern(hero) == Distance.One)
					{
						plans.Add(PlanType.Stay);
					}
				}

				// If beside a Tavern, 
				// or weak, 
				// check if usefull.
				else if (health < (Hero.HealthBattle << 1) || (map.GetDistanceToTavern(hero) == Distance.One && health < Hero.HealthMax - Hero.HealthBattle))
				{
					plans.Add(PlanType.ToTavern);
				}
				
				// only when we can conquer it.
				if (health > Hero.HealthBattle)
				{
					plans.Add(PlanType.ToMineClosetToTavern);
					plans.Add(PlanType.ToMine);
					plans.Add(PlanType.ToFreeMine);

					if (mines > 1)
					{
						plans.Add(PlanType.ToOwnMine);
					}
				}
			}
			return plans;
		}

		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Turn: {0} Player: {1}, Children: {2}, Score: {3}",
					this.Turn,
					(int)this.PlayerToMove,
					this.Children.Count,
					this.Score.ToConsoleDisplay(this.PlayerToMove));
			}
		}
	}
}
