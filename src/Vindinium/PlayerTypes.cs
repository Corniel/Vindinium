using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Vindinium
{
	/// <summary>Extra move players.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct PlayerTypes
	{
		/// <summary>Gets all player types.</summary>
		public static PlayerType[] All = new PlayerType[] { PlayerType.Hero1, PlayerType.Hero2, PlayerType.Hero3, PlayerType.Hero4 };

		/// <summary>Gets the others for a player type.</summary>
		public static readonly Dictionary<PlayerType, PlayerType[]> Other = new Dictionary<PlayerType, PlayerType[]>()
		{
			{ PlayerType.Hero1, new PlayerType[]{ PlayerType.Hero2, PlayerType.Hero3, PlayerType.Hero4 } },
			{ PlayerType.Hero2, new PlayerType[]{ PlayerType.Hero3, PlayerType.Hero4, PlayerType.Hero1 } },
			{ PlayerType.Hero3, new PlayerType[]{ PlayerType.Hero4, PlayerType.Hero1, PlayerType.Hero2 } },
			{ PlayerType.Hero4, new PlayerType[]{ PlayerType.Hero1, PlayerType.Hero2, PlayerType.Hero3 } },
		};

		public static readonly PlayerTypes None = default(PlayerTypes);

		public static readonly PlayerTypes Hero1 = new PlayerTypes(PlayerType.Hero1);
		public static readonly PlayerTypes Hero2 = new PlayerTypes(PlayerType.Hero2);
		public static readonly PlayerTypes Hero3 = new PlayerTypes(PlayerType.Hero3);
		public static readonly PlayerTypes Hero4 = new PlayerTypes(PlayerType.Hero4);

		public PlayerTypes(params PlayerType[] players)
		{
			m_Value = PlayerTypes.ToByte(players);
		}
		public PlayerTypes(IEnumerable<PlayerType> players)
		{
			m_Value = PlayerTypes.ToByte(players);
		}

		private byte m_Value;

		public int Length { get { return Bits.Count(m_Value); } }

		public bool IsEmpty() { return m_Value == default(byte); }

		public bool Contains(PlayerType direction)
		{
			return (m_Value & (1 << (int)direction)) != 0;
		}

		public PlayerType[] ToArray()
		{
			var copy = this;
			return All
				.Where(direction => copy.Contains(direction))
				.ToArray();
		}

		public override string ToString() { return String.Join("|", ToArray()); }

		public string DebuggerDisplay { get { return IsEmpty() ? "<none>" : ToString(); } }

		public override int GetHashCode() { return m_Value; }
		public override bool Equals(object obj) { return base.Equals(obj); }

		private static byte ToByte(IEnumerable<PlayerType> players)
		{
			byte val = 0;
			foreach (var direction in players)
			{
				val |= (byte)(1 << (int)direction);
			}
			return val;
		}
	}
}
