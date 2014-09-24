using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;

namespace Vindinium
{
	[DebuggerDisplay("{DebugToString()}")]
	public struct MineOwnership
	{
		public static readonly MineOwnership Empty = default(MineOwnership);

		private ulong m_Value;
		private uint m_Index;

		public PlayerType this[int index]
		{
			get
			{
				if ((m_Index & Bits.GetFlagMaskUInt32(index)) == 0)
				{
					return PlayerType.None;
				}
				var val = 3 & (m_Value >> (index << 1));
				switch (val)
				{
					default:
					case 0: return PlayerType.Hero1;
					case 1: return PlayerType.Hero2;
					case 2: return PlayerType.Hero3;
					case 3: return PlayerType.Hero4;
				}
			}
		}

		public MineOwnership Set(int index, PlayerType player)
		{
			var index2 = index << 1;

			var mines = new MineOwnership()
			{
				// copy the value but clear the index spot.
				m_Value = m_Value & (UInt64.MaxValue ^ (3UL << index2)),
				m_Index = m_Index,
			};
			
			// remove all.
			if (player == PlayerType.None)
			{
				mines.m_Index &= Bits.GetUnflagMaskUInt32(index);
			}
			else
			{
				mines.m_Index |= Bits.GetFlagMaskUInt32(index);

				switch (player)
				{
					case PlayerType.Hero2: mines.m_Value |= 1UL << index2; break;
					case PlayerType.Hero3: mines.m_Value |= 2UL << index2; break;
					case PlayerType.Hero4: mines.m_Value |= 3UL << index2; break;
				}
			}
			return mines;
		}
		public MineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner, int mineLength)
		{
#if DEBUG
			if (curOwner == PlayerType.None) { throw new ArgumentException("The current owner should be set.", "curOwner"); }
#endif
			var mines = new MineOwnership()
			{
				m_Value = m_Value,
				m_Index = m_Index,
			};

			if (newOwner == PlayerType.None)
			{
				for (int index = 0; index < mineLength; index++)
				{
					mines.m_Value &= (UInt64.MaxValue ^ (3UL << (index << 1)));
					mines.m_Index &= Bits.GetUnflagMaskUInt32(index);
				}
			}
			else
			{
				for (int index = 0; index < mineLength; index++)
				{
					var index2 = index << 1;
					mines.m_Value &= (UInt64.MaxValue ^ (3UL << index2));
					mines.m_Index &= Bits.GetUnflagMaskUInt32(index);

					switch (newOwner)
					{
						case PlayerType.Hero2: mines.m_Value |= 1UL << index2; break;
						case PlayerType.Hero3: mines.m_Value |= 2UL << index2; break;
						case PlayerType.Hero4: mines.m_Value |= 3UL << index2; break;
					}
				}
			}
			return mines;
		}

		public int Count(PlayerType player)
		{
			var count = 0;
			for (int index = 0; index < 32; index++)
			{
				if (this[index] == player)
				{
					index++;
				}
			}
			return count;
		}
	
		public string DebugToString()
		{
			var sb = new StringBuilder();

			for (int index = 0; index < 32; index++)
			{
				switch (this[index])
				{
					default:
					case PlayerType.None:  sb.Append('.'); break;
					case PlayerType.Hero1: sb.Append('1'); break;
					case PlayerType.Hero2: sb.Append('2'); break;
					case PlayerType.Hero3: sb.Append('3'); break;
					case PlayerType.Hero4: sb.Append('4'); break;
				}
			}
			return sb.ToString();
		}

		/// <summary>Creates mine ownership from tiles.</summary>
		public static MineOwnership CreateFromTiles(string tiles)
		{
			var mines = MineOwnership.Empty;

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

		public static MineOwnership Create(params int[] mines)
		{
			return Create(mines.Select(mine => (PlayerType)mine).ToArray());
		}

		public static MineOwnership Create(params PlayerType[] mines)
		{
			var ownership = MineOwnership.Empty;

			for (int index = 0; index < mines.Length; index++)
			{
				ownership = ownership.Set(index, mines[index]);
			}
			return ownership;
		}
	}
}
