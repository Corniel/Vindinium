using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking
{
	[Flags]
	public enum TileClaim
	{
		None = 0,
		Hero1 = 1,
		Hero2 = 2,
		Hero3 = 4,
		Hero4 = 8,
		Mine = 16,
		Taverne = 32,
	}
	public static class TileClaims
	{
		public static TileClaim ToTileClaim(this PlayerType player)
		{
			switch (player)
			{
				default:
				case PlayerType.None: return TileClaim.None;
				case PlayerType.Hero1: return TileClaim.Hero1;
				case PlayerType.Hero2: return TileClaim.Hero2;
				case PlayerType.Hero3: return TileClaim.Hero3;
				case PlayerType.Hero4: return TileClaim.Hero4;
			}
		}

		public static string ToUnitTestString(this TileClaim claim)
		{
			switch (claim)
			{
				case TileClaim.Hero1: return "1";
				case TileClaim.Hero2: return "2";
				case TileClaim.Hero3: return "3";
				case TileClaim.Hero4: return "4";
				case TileClaim.Mine: return "M";
				case TileClaim.Taverne: return "T";
				
				case TileClaim.None: 
				default: return " ";
			}
		}
	}
}
