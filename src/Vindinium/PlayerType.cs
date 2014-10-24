using System.Collections.Generic;

namespace Vindinium
{
	/// <summary>Represents the different player types.</summary>
	public enum PlayerType
	{
		None = 0,
		Hero1 = 1,
		Hero2 = 2,
		Hero3 = 3,
		Hero4 = 4,
	}

	public static class PlayerTypeExensions
	{
		/// <summary>Gets the others for a player type.</summary>
		public static PlayerType Next(this PlayerType playerToMove)
		{
			return PlayerTypes.Other[playerToMove][0];
		}

		public static bool IsOppo(this PlayerType player, PlayerType check)
		{
			return player != check && player > PlayerType.None;
		}
	}
}
