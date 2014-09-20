using System;

namespace Vindinium
{
	/// <summary>Represents distances.</summary>
	public static class Distances
	{
		/// <summary>Creates distances.</summary>
		public static Distance[,] Create(int size)
		{
			return Create(size, size);
		}

		/// <summary>Creates distances.</summary>
		public static Distance[,] Create(int rows, int cols)
		{
			return new Distance[rows, cols];
		}

		/// <summary>Clears the distances.</summary>
		public static void Clear(this Distance[,] distances)
		{
			Array.Clear(distances, 0, distances.Length);
		}
	}
}
