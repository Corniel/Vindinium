using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class Node
	{
		public Node(State state)
		{
			this.State = state;
			this.Score = Score.Create(state);

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

		public static void ClearLookup()
		{
			Lookup.Clear();
		}

		public Score Score { get; protected set; }
		public State State { get; protected set; }
		public int Turn { get { return this.State.Turn; } }
		public PlayerType PlayerToMove { get { return this.State.PlayerToMove; } }
		
		public Dictionary<PlayerType, List<Move>> Moves { get; protected set; }

		public List<Node> Children { get; set; }

		public void Process(Map map, int turn)
		{
			if (this.Turn >= 1999) { return; }
			if (turn == this.Turn && this.Children.Count != 1) { return; }

			var moves = this.Moves[this.PlayerToMove];

			if (this.Children.Count == 0 || moves.Count == 0)
			{
				var player = this.PlayerToMove;
				var hero = this.State.GetHero(player);
				var source = map[hero];

				var health = hero.Health;
				var mines = hero.Mines;

				if (moves.Count == 0)
				{
					var plans = GeneratePlans(map, hero, player);
					foreach (var plan in plans)
					{
						MovesGenerator.Instance.AddMoves(moves, map, this.State, source, hero, player, plan);
					}
				}
				foreach (var move in moves)
				{
					var target = move.GetTarget(source);
					if (target != null)
					{
						var state = this.State.Move(map, hero, player, source, target);
						var child = Node.Get(this.Turn + 1, map, state);

						var heroNew = state.GetHero(player);

						// if no tarverne hit, if no mine changes, and no enermies as neigbhor.
						if (heroNew.Health <= health &&
							heroNew.Mines == mines &&
							target.Neighbors.All(t => state.GetOccupied(t, player) == PlayerType.None))
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

			if (this.Children.Count == 1)
			{
				this.Children[0].Process(map, turn + 1);
			}
			else
			{
				for (int i = 0; i < this.Children.Count;i++ )
				{
					var child = this.Children[i];
					child.Process(map, turn);
				}
				Children.Sort(NodeComparer.Get(this.PlayerToMove));
			}
			this.Score = this.Children[0].Score;
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
			foreach (var player in PlayerTypes.All)
			{
				this.Moves[player].Clear();
			}
		}

		public List<PlanType> GeneratePlans(Map map, Hero hero, PlayerType player)
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
				// Combat, nothing else.
				if (source.Neighbors.Any(tile => this.State.GetOccupied(tile, player) != PlayerType.None))
				{
					plans.Add(PlanType.Flee);
					//plans.Add(PlanType.Attack);
				}
				else if (health < Hero.HealthBattle)
				{
					plans.Add(PlanType.ToTaverne);
					// kill yourself.
					//plans.Add(PlanType.Attack);
					//plans.Add(PlanType.Flee);
				}
				else
				{
					// If beside a taverne, check if usefull.
					if (source.Neighbors.Any(tile => tile.IsTaverne))
					{
						plans.Add(PlanType.ToTaverne);
					}

					plans.Add(PlanType.ToOppoMine);
					plans.Add(PlanType.ToFreeMine);
					if (hero.Mines >= (map.Mines.Length >> 1))
					{
						plans.Add(PlanType.ToOwnMine);
					}
					//if (PlayerTypes.Other[player].Select(tp => this.State.GetHero(tp)).Any(oppo => oppo.Health + Hero.HealthBattle < health))
					//{
					//	plans.Add(PlanType.Attack);
					//}
					//else
					//{
					//	plans.Add(PlanType.Flee);
					//}
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
					this.Score.DebuggerDisplay);
			}
		}
	}
}
