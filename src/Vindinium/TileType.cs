using System;

namespace Vindinium
{
	public enum TileType
	{
		Unknown = 0,
		Hero1 = 1,
		Hero2 = 2,
		Hero3 = 3,
		Hero4 = 4,
		GoldMine1 = 5,
		GoldMine2 = 6,
		GoldMine3 = 7,
		GoldMine4 = 8,
		GoldMine = 9,
		Empty = 10,
		Taverne = 11,
		Impassable = 12,
		 
	}

	public static class TileTypes
	{
		/// <summary>Tries to parse a string to a tile type.</summary>
		public static TileType TryParse(String str)
		{
			switch (str)
			{
				case "@1": return TileType.Hero1;
				case "@2": return TileType.Hero2;
				case "@3": return TileType.Hero3;
				case "@4": return TileType.Hero4;

				case "  ": return TileType.Empty;

				case "$-": return TileType.GoldMine;
				case "$1": return TileType.GoldMine1;
				case "$2": return TileType.GoldMine2;
				case "$3": return TileType.GoldMine3;
				case "$4": return TileType.GoldMine4;

				case "[]": return TileType.Taverne;

				case "##": return TileType.Impassable;

				default: return TileType.Unknown;
			}
		}
	}
}
