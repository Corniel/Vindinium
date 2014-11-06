using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium
{
	public static class MultipleArrayExtensions
	{
		/// <summary>Creates distances.</summary>
		public static T[,] Create<T>(int size)
		{
			return Create<T>(size, size);
		}

		/// <summary>Creates distances.</summary>
		public static T[,] Create<T>(int rows, int cols)
		{
			return new T[rows, cols];
		}

		public static T Get<T>(this T[,] map, Tile tile)
		{
			return map[tile.X, tile.Y];
		}
		public static T Get<T>(this T[,] map, Hero hero)
		{
			return map[hero.X, hero.Y];
		}

		public static void Set<T>(this T[,] map, Tile tile, T val)
		{
			map[tile.X, tile.Y] = val;
		}

		/// <summary>Clears the distances.</summary>
		public static void Clear<T>(this T[,] distances)
		{
			Array.Clear(distances, 0, distances.Length);
		}
	}
}
