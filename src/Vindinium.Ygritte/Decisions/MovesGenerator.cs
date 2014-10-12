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

				case PlanType.Flee: GetFleeMoves(moves, map, state, source, hero, player); return;
				case PlanType.Attack: GetAttackMoves(moves, map, state, source, hero, player); return;
				
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

		private void GetAttackMoves(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			foreach (var other in PlayerTypes.Other[player])
			{
				var oppo = state.GetHero(other);
				// some what close, and not covered by a taverne.
				if (Map.GetManhattanDistance(hero, oppo) < 5 && map.GetDistanceToTaverne(oppo) > Distance.One)
				{
					var move = new MoveAttack(other);
				}
			}
		}
		private void GetFleeMoves(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			foreach (var other in PlayerTypes.Other[player])
			{
				var oppo = state.GetHero(other);
				// some what close.
				if (Map.GetManhattanDistance(hero, oppo) < 5)
				{
					var move = new MoveFlee(other);
				}
			}
		}
	}
}
