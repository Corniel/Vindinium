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

				case PlanType.ToOppoMine: GetOppoMinePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToFreeMine: GetFreeMinePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToOwnMine: GetOwnMinePaths(moves, map, state, source, hero, player); return;

				case PlanType.ToTaverne: GetTavernePaths(moves, map, state, source, hero, player); return;

				case PlanType.Flee:
				case PlanType.Attack: GetDirectionsPaths(moves, map, state, source, hero, player); return;
				
				default: break;
			}
		}

		private void GetDirectionsPaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			foreach (var target in source.Neighbors)
			{
				moves.Add(new SingleMove(source, target));
			}
		}

		private void GetOppoMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex].IsOppo(player))
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
	}
}
