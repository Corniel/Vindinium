using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Vindinium
{
	[DebuggerDisplay("{DebugToString()}")]
	public struct MineOwnership20 : IMineOwnership, IEquatable<MineOwnership20>
	{
		public const int IndexMax = 20;
		public static readonly MineOwnership20 Empty = default(MineOwnership20);

		private ulong m_Value;

		public PlayerType this[int index]
		{
			get
			{
				ulong val = 0;
				switch (index)
				{
					case 00: val = m_Value & 7; break;
					case 01: val = (m_Value >> 03) & 7; break;
					case 02: val = (m_Value >> 06) & 7; break;
					case 03: val = (m_Value >> 09) & 7; break;
					case 04: val = (m_Value >> 12) & 7; break;
					case 05: val = (m_Value >> 15) & 7; break;
					case 06: val = (m_Value >> 18) & 7; break;
					case 07: val = (m_Value >> 21) & 7; break;
					case 08: val = (m_Value >> 24) & 7; break;
					case 09: val = (m_Value >> 27) & 7; break;
					case 10: val = (m_Value >> 30) & 7; break;
					case 11: val = (m_Value >> 33) & 7; break;
					case 12: val = (m_Value >> 36) & 7; break;
					case 13: val = (m_Value >> 39) & 7; break;
					case 14: val = (m_Value >> 42) & 7; break;
					case 15: val = (m_Value >> 45) & 7; break;
					case 16: val = (m_Value >> 48) & 7; break;
					case 17: val = (m_Value >> 51) & 7; break;
					case 18: val = (m_Value >> 54) & 7; break;
					case 19: val = (m_Value >> 57) & 7; break;
					default: throw new ArgumentOutOfRangeException("index", "Index should be in the range [0, 19].");
				}
				switch (val)
				{
					default: return PlayerType.None;
					case 1: return PlayerType.Hero1;
					case 2: return PlayerType.Hero2;
					case 3: return PlayerType.Hero3;
					case 4: return PlayerType.Hero4;
				}
			}

		}
		public IMineOwnership Set(int index, PlayerType player)
		{
			unchecked
			{
				var mines = new MineOwnership20() { m_Value = m_Value };

				var shift = index * 3;
				var clear = ulong.MaxValue ^ (7UL << shift);

				mines.m_Value &= clear;
				mines.m_Value |= ((ulong)player) << shift;

				return mines;
			}
		}
		public IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner, int length)
		{
#if DEBUG
			if (curOwner == PlayerType.None) { throw new ArgumentException("The current owner should be set.", "curOwner"); }
#endif
			unchecked
			{
				var mines = new MineOwnership20() { m_Value = m_Value };

				var shift = 0;
				var nwo = (ulong)newOwner;

				for (int index = 0; index < length; index++)
				{
					var owner = mines[index];
					if (owner == curOwner)
					{
						var clear = ulong.MaxValue ^ (7UL << shift);
						mines.m_Value &= clear;
						mines.m_Value |= nwo << shift;
					}

					shift += 3;
				}
				return mines;
			}
		}

		public int Count(PlayerType player)
		{
			return Count(player, IndexMax);
		}
		public int Count(PlayerType player, int length)
		{
			unchecked
			{
				var count = 0;
				for (int index = 0; index < length; index++)
				{
					if (this[index] == player)
					{
						count++;
					}
				}
				return count;
			}
		}

		public string DebugToString()
		{
			var sb = new StringBuilder();

			for (int index = 0; index < IndexMax; index++)
			{
				switch (this[index])
				{
					default:
					case PlayerType.None: sb.Append('.'); break;
					case PlayerType.Hero1: sb.Append('1'); break;
					case PlayerType.Hero2: sb.Append('2'); break;
					case PlayerType.Hero3: sb.Append('3'); break;
					case PlayerType.Hero4: sb.Append('4'); break;
				}
			}
			return sb.ToString();
		}

		/// <summary>Creates mine ownership from tiles.</summary>
		public IMineOwnership UpdateFromTiles(string tiles)
		{
			IMineOwnership mines = MineOwnership20.Empty;

			var index = 0;

			for (int p = 0; p < tiles.Length; p += 2)
			{
				if (tiles[p] == '$')
				{
					switch (tiles[p + 1])
					{
						case '1': mines = mines.Set(index, PlayerType.Hero1); break;
						case '2': mines = mines.Set(index, PlayerType.Hero2); break;
						case '3': mines = mines.Set(index, PlayerType.Hero3); break;
						case '4': mines = mines.Set(index, PlayerType.Hero4); break;
						case '-':
						default: break;
					}
					index++;
				}
			}
			return mines;
		}

		public static IMineOwnership Create(params int[] mines)
		{
			return Create(mines.Select(mine => (PlayerType)mine).ToArray());
		}
		public static IMineOwnership Create(params PlayerType[] mines)
		{
			IMineOwnership ownership = MineOwnership20.Empty;

			for (int index = 0; index < mines.Length; index++)
			{
				if (mines[index] != PlayerType.None)
				{
					ownership = ownership.Set(index, mines[index]);
				}
			}
			return ownership;
		}

		public override int GetHashCode() { return m_Value.GetHashCode(); }

		public override bool Equals(object obj) { return base.Equals(obj); }
		public bool Equals(MineOwnership20 other)
		{
			return this.m_Value == other.m_Value;
		}

		public static bool operator ==(MineOwnership20 l, MineOwnership20 r)
		{
			return l.Equals(r);
		}
		public static bool operator !=(MineOwnership20 l, MineOwnership20 r)
		{
			return !(l == r);
		}
	}
}
