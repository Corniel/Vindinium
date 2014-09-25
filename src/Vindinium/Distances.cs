using System;
using System.Text;

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

		public static string ToUnitTestString(this Distance[,] distances)
		{
			var sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine(new string('-', distances.GetLength(0) * 3 + 1));

			for (var y = 0; y < distances.GetLength(1); y++)
			{
				sb.Append('|');
				for (var x = 0; x < distances.GetLength(0); x++)
				{
					sb.AppendFormat("{0,2}|", distances[x, y]);
				}
				sb.AppendLine();
				sb.AppendLine(new string('-', distances.GetLength(0) * 3 + 1));
			}
			return sb.ToString();
		}
	}
}
