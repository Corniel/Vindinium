using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	public class MovesGenerator
	{
		private Dictionary<Tile, Distance[,]> paths = new Dictionary<Tile, Distance[,]>();

		public void Clear()
		{
			paths.Clear();
		}

		private Distance[,] Get(Map map, Tile tile)
		{
			Distance[,] distances;
			if (!paths.TryGetValue(tile, out distances))
			{
				distances = map.GetDistances(tile);
				paths[tile] = distances;
			}
			return distances;
		}

		public void AddPaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player, PlanType plan)
		{
			switch (plan)
			{

				case PlanType.Crashed: moves.Add(new MoveCrashed()); break;

				case PlanType.ToOppoMine: GetOppoMinePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToFreeMine: GetFreeMinePaths(moves, map, state, source, hero, player); return;
				case PlanType.ToOwnMine: GetOwnMinePaths(moves, map, state, source, hero, player); return;

				case PlanType.ToTaverne: GetTavernePaths(moves, map, state, source, hero, player); return;

				case PlanType.Flee:
					break;
				case PlanType.Attack:
					break;
				
				default: break;
			}
		}

		private void GetOppoMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex].IsOppo(player))
				.Select(t => Get(map, t))
				.OrderBy(d => d.Get(source))
				.Take(2);

			moves.AddRange(p.Select(m => new MoveFromPath(m)));
		}
		private void GetFreeMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex] == PlayerType.None)
				.Select(t => Get(map, t))
				.OrderBy(d => d.Get(source))
				.Take(2);

			moves.AddRange(p.Select(m => new MoveFromPath(m)));
		}
		private void GetOwnMinePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Mines
				.Where(m => state.Mines[m.MineIndex] == player)
				.Select(t => Get(map, t))
				.OrderBy(d => d.Get(source))
				.Take(1);

			moves.AddRange(p.Select(m => new MoveFromPath(m)));
		}

		private void GetTavernePaths(List<Move> moves, Map map, State state, Tile source, Hero hero, PlayerType player)
		{
			var p = map.Tavernes
				.Select(t => Get(map, t))
				.OrderBy(d => d.Get(source))
				.Take(2);

			moves.AddRange(p.Select(m => new MoveFromPath(m)));
		}
	}
}
