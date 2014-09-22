using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Vindinium.Serialization
{
	[DataContract, DebuggerDisplay("{DebugToString()}")]
	public class Board
	{
		[DataMember]
		public int size;

		[DataMember]
		public string tiles;

		/// <summary>Represents the board as debug string.</summary>
		public string DebugToString()
		{
			return String.Format("Size: {0}, Tiles: {1}", size, tiles);
		}

		/// <summary>Gets the tiles per row.</summary>
		public string[] ToRows()
		{
			var rows = new string[size];

			var row = 0;

			if (tiles != null && size > 0)
			{
				for (int i = 0; i < tiles.Length; i += size * 2)
				{
					rows[row++] = tiles.Substring(i, size * 2);
				}
			}
			return rows;
		}
	}
}
