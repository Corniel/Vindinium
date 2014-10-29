using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vindinium
{
	/// <summary>Extra move players.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct PlayerTypes : IEnumerable<PlayerType>
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

		/// <summary>Adds a player to the players. </summary>
		public PlayerTypes Add(PlayerType player)
		{
			var val = m_Value;
			val |= (byte)(1 << (int)player);
			return new PlayerTypes() { m_Value = val };
		}
		/// <summary>Removes a player from the players. </summary>
		public PlayerTypes Remove(PlayerType player)
		{
			var val = m_Value;
			val ^= (byte)(1 << (int)player);
			return new PlayerTypes() { m_Value = val };
		}

		public bool Contains(PlayerType player)
		{
			return (m_Value & (1 << (int)player)) != 0;
		}

		public PlayerType[] ToArray() { return GetItems().ToArray(); }

		public override string ToString() { return String.Join("|", ToArray()); }

		public string DebuggerDisplay { get { return IsEmpty() ? "<none>" : ToString(); } }

		public override int GetHashCode() { return m_Value; }
		public override bool Equals(object obj) { return base.Equals(obj); }

		private static byte ToByte(IEnumerable<PlayerType> players)
		{
			byte val = 0;
			foreach (var player in players.Where(p => p != PlayerType.None))
			{
				val |= (byte)(1 << (int)player);
			}
			return val;
		}

		public IEnumerator<PlayerType> GetEnumerator() { return GetItems().GetEnumerator(); }

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() { return GetItems().GetEnumerator(); }

		private IEnumerable<PlayerType> GetItems()
		{
			foreach (var tp in All)
			{
				if (Contains(tp)) { yield return tp; }
			}
		}
	}
}
