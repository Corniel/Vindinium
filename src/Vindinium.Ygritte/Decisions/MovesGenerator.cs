using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	public class MovesGenerator
	{
		public static readonly MovesGenerator Instance = new MovesGenerator();

		private Dictionary<Tile, MoveFromPath> paths = new Dictionary<Tile, MoveFromPath>();

		public void Clear()
		{
			paths.Clear();
		}

		public Distance[,] GetDistances(Map map, Tile tile)
		{
			var moves = GetMoveFromPath(map, tile);
			return moves.Distances;
		}

		private MoveFromPath GetMoveFromPath(Map map, Tile tile)
		{
			MoveFromPath move;
			if (!paths.TryGetValue(tile, out move))
			{
				move = new MoveFromPath(map.GetDistances(tile));
				paths[tile] = move;
			}
			return move;
		}

		public void AddMoves(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player, PlanType plan)
		{
			switch (plan)
			{

				case PlanType.Crashed: moves.Add(Move.Crashed); break;
				case PlanType.Stay: moves.Add(Move.Stay); break;

				case PlanType.ToMineClosetToTaverne: GetToMineClosetToTavernePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToMine: GetMinePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToFreeMine: GetFreeMinePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToOwnMine: GetOwnMinePaths(moves, map, state, source, hero, player); return;

				case PlanType.ToTaverne: GetTavernePaths(moves, map, state, source, hero, player); return;

				case PlanType.Combat: GetCombatMoves(moves, map, state, source, hero, player); return;
				
				default: break;
			}
		}

		private void GetToMineClosetToTavernePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex] != player)
				.OrderBy(m => map.GetDistanceToTaverne(m))
				.Select(t => GetMoveFromPath(map, t))
				.Take(1);
			moves.AddRange(p);
		}

		private void GetMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex] != player)
				.Select(t => GetMoveFromPath(map, t))
				.OrderBy(d => d.GetDistance(source))
				.Take(2);
			moves.AddRange(p);
		}
		private void GetFreeMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex] == PlayerType.None)
				.Select(t => GetMoveFromPath(map, t))
				.OrderBy(d => d.GetDistance(source))
				.Take(2);
			moves.AddRange(p);
		}
		private void GetOwnMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex] == player)
				.Select(t => GetMoveFromPath(map, t))
				.OrderBy(d => d.GetDistance(source))
				.Take(1);
			moves.AddRange(p);
		}

		private void GetTavernePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Tavernes
				.Select(t => GetMoveFromPath(map, t))
				.OrderBy(d => d.GetDistance(source))
				.Take(2);
			moves.AddRange(p);
		}

		private void GetCombatMoves(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			foreach (var other in PlayerTypes.Other[player])
			{
				var oppo = state.GetHero(other);
				var distance = Map.GetManhattanDistance(hero, oppo);
				
				// some what close.
				if (distance <= Node.CombatDistance)
				{
					var heroToTaverne = map.GetDistanceToTaverne(hero);
					var oppoToTaverne = map.GetDistanceToTaverne(oppo);
					var heroHealth = hero.Health;
					var oppoHealth = oppo.Health;
					var target = map[oppo];
					// We hit first, so we have an extra try
					var isOddDistance = (distance & 2) == 1;
					var hitsNeaded = (oppoHealth + Hero.HealthBattle - 1)/ Hero.HealthBattle;
					var hitsPossible = (heroHealth + Hero.HealthBattle - 1)/ Hero.HealthBattle + (isOddDistance ? 1 : 0);

					// are we interested in attacking?
					// 1. We can kill the hero, and he has at least one mine or not on his own span, and he is not protected by a taverne.
					if (oppoToTaverne > 1 && (oppo.Mines > 0 || target != map.GetSpawn(other)) && hitsPossible >= hitsNeaded)
					{
						moves.Add(new MoveAttack(other));
						
						// We are standing next to a taverne, we would like to investigate staying or drinking too.
						if (heroToTaverne == 1)
						{
							var taverne = map.Mines.FirstOrDefault(m => Map.GetManhattanDistance(hero, m) == 1);
							moves.Add(GetMoveFromPath(map, taverne));
							moves.Add(Move.Stay);
						}
						moves.Add(new MoveFlee(other));
					}
					// 2. We have no mines, and it looks like we will be killed anyway.
					else if (heroToTaverne > 1 && hero.Mines == 0 && heroToTaverne > oppoToTaverne && hitsNeaded > hitsPossible)
					{
						moves.Add(new MoveAttack(other));
						moves.Add(new MoveFlee(other));
					}
					// We prefer to flee.
					else
					{
						// We are standing next to a taverne, we would like to investigate staying or drinking too.
						if (heroToTaverne == 1)
						{
							var taverne = map.Mines.FirstOrDefault(m => Map.GetManhattanDistance(hero, m) == 1);
							moves.Add(GetMoveFromPath(map, taverne));
							moves.Add(Move.Stay);
						}
						moves.Add(new MoveFlee(other));
						moves.Add(new MoveAttack(other));
					}
				}
			}
		}
	}
}
