using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Decisions
{
	public class PathGenerator
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

		public void AddPaths(List<Distance[,]> paths, Map map, State state, PlanType plan)
		{
			switch (plan)
			{
				
				case PlanType.ToOppoMine:



					break;
				case PlanType.ToFreeMine: GetFreeMinePaths(paths, map, state); return;

				case PlanType.ToOwnMine:
					break;
				case PlanType.ToTaverne:
					break;
				case PlanType.Flee:
					break;
				case PlanType.Attack:
					break;

				case PlanType.Crashed:
				default: break;
			}
		}

		private void GetFreeMinePaths(List<Distance[,]> paths, Map map, State state)
		{
			
		}
	}
}
