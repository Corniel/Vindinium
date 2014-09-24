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

	public static class PlayerTypes
	{
		/// <summary>Gets all player types.</summary>
		public static PlayerType[] All = new PlayerType[] { PlayerType.Hero1, PlayerType.Hero2, PlayerType.Hero3, PlayerType.Hero4 };

		/// <summary>Gets the others for a player type.</summary>
		public static readonly Dictionary<PlayerType, PlayerType[]> Other = new Dictionary<PlayerType, PlayerType[]>()
		{
			{ PlayerType.Hero1, new PlayerType[]{ PlayerType.Hero2, PlayerType.Hero3, PlayerType.Hero4 } },
			{ PlayerType.Hero2, new PlayerType[]{ PlayerType.Hero1, PlayerType.Hero3, PlayerType.Hero4 } },
			{ PlayerType.Hero3, new PlayerType[]{ PlayerType.Hero1, PlayerType.Hero2, PlayerType.Hero4 } },
			{ PlayerType.Hero4, new PlayerType[]{ PlayerType.Hero1, PlayerType.Hero2, PlayerType.Hero3 } },
		};

		/// <summary>Gets the others for a player type.</summary>
		public static readonly Dictionary<PlayerType, PlayerType> Next = new Dictionary<PlayerType, PlayerType>()
		{
			{ PlayerType.Hero1, PlayerType.Hero2  },
			{ PlayerType.Hero2, PlayerType.Hero3  },
			{ PlayerType.Hero3, PlayerType.Hero4  },
			{ PlayerType.Hero4, PlayerType.Hero1  },
		};
	}
}
