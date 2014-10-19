using System.Text;

namespace Vindinium
{
	/// <summary>Represents distances.</summary>
	public static class Distances
	{
		/// <summary>Creates distances.</summary>
		public static Distance[,] Create(int size)
		{
			return MultipleArrayExtensions.Create<Distance>(size);
		}

		/// <summary>Creates distances.</summary>
		public static Distance[,] Create(int rows, int cols)
		{
			return MultipleArrayExtensions.Create<Distance>(rows, cols);
		}

		/// <summary>Creates distances.</summary>
		public static Distance[,] Create(Map map)
		{
			return Create(map.Width, map.Height);
		}

		/// <summary>Represents the distance array as unit test string.</summary>
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
					sb.AppendFormat("{0,2}|", distances[x, y] == Distance.Unknown ? "" : distances[x, y].ToString());
				}
				sb.AppendLine();
				sb.AppendLine(new string('-', distances.GetLength(0) * 3 + 1));
			}
			return sb.ToString();
		}
	}
}
