using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium
{
	public static class TileExtensions
	{
		public static IEnumerable<Tile> GetPassables(this IEnumerable<Tile> tiles)
		{
			return tiles.Where(n => n.IsPassable);
		}
	}
}
