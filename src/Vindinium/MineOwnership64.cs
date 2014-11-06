using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Vindinium
{
	[DebuggerDisplay("{ToString()}")]
	public struct MineOwnership64 : IMineOwnership, IEquatable<MineOwnership64>
	{
		public const int IndexMax = 64;
		public static readonly MineOwnership64 Empty = default(MineOwnership64);

		private ulong m_Index;
		private ulong m_Value0;
		private ulong m_Value1;

		public PlayerType this[int index]
		{
			get
			{
				if ((m_Index & Bits.GetFlagMaskUInt64(index)) == 0)
				{
					return PlayerType.None;
				}
				ulong val = 0;
				switch (index >> 5)
				{
					case 01: val = 3 & (m_Value1 >> ((31 & index) << 1)); break;
					default: val = 3 & (m_Value0 >> (index << 1)); break;
				}
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

		public IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner)
		{
			return ChangeOwnership(curOwner, newOwner, IndexMax);
		}
		public IMineOwnership Set(int index, PlayerType player)
		{
			var index2 = (31 & index) << 1;
			var bank = index >> 5;

			var mines = new MineOwnership64()
			{
				// copy the value but clear the index spot.
				m_Value0 = m_Value0,
				m_Value1 = m_Value1,
				m_Index = m_Index,
			};
			switch (bank)
			{
				case 01: mines.m_Value1 &= (UInt64.MaxValue ^ (3UL << index2)); break;
				default: mines.m_Value0 &= (UInt64.MaxValue ^ (3UL << index2)); break;
			}

			// remove all.
			if (player == PlayerType.None)
			{
				mines.m_Index &= Bits.GetFlagMaskUInt64(index);
			}
			else
			{
				mines.m_Index |= Bits.GetFlagMaskUInt64(index);

				switch (bank)
				{
					case 1:
						switch (player)
						{
							case PlayerType.Hero2: mines.m_Value1 |= 1UL << index2; break;
							case PlayerType.Hero3: mines.m_Value1 |= 2UL << index2; break;
							case PlayerType.Hero4: mines.m_Value1 |= 3UL << index2; break;
						}
						break;
					default:
						switch (player)
						{
							case PlayerType.Hero2: mines.m_Value0 |= 1UL << index2; break;
							case PlayerType.Hero3: mines.m_Value0 |= 2UL << index2; break;
							case PlayerType.Hero4: mines.m_Value0 |= 3UL << index2; break;
						}
						break;
				}
			}
			return mines;
		}
		public IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner, int mineLength)
		{
#if DEBUG
			if (curOwner == PlayerType.None) { throw new ArgumentException("The current owner should be set.", "curOwner"); }
#endif
			var mines = new MineOwnership64()
			{
				m_Value0 = m_Value0,
				m_Value1 = m_Value1,
				m_Index = m_Index,
			};

			if (newOwner == PlayerType.None)
			{
				for (int index = 0; index < mineLength; index++)
				{
					var shft = (31 & index) << 1;
					var bank = index >> 5;

					PlayerType cur = PlayerType.None;
					switch (bank)
					{
						case 01: cur = (PlayerType)(1 + (3 & (mines.m_Value1 >> shft))); break;
						default: cur = (PlayerType)(1 + (3 & (mines.m_Value0 >> shft))); break;
					}

					if (cur == curOwner)
					{
						switch (bank)
						{
							case 01: mines.m_Value1 &= (UInt64.MaxValue ^ (3UL << shft)); break;
							default: mines.m_Value0 &= (UInt64.MaxValue ^ (3UL << shft)); break;
						}
						mines.m_Index &= Bits.GetUnflagMaskUInt64(index);
					}
				}
			}
			else
			{
				for (int index = 0; index < mineLength; index++)
				{
					var shft = (31 & index) << 1;
					var bank = index >> 5;

					PlayerType cur = PlayerType.None;
					switch (bank)
					{
						case 01: cur = (PlayerType)(1 + (3 & (mines.m_Value1 >> shft))); break;
						default: cur = (PlayerType)(1 + (3 & (mines.m_Value0 >> shft))); break;
					}

					if (cur == curOwner)
					{
						switch (bank)
						{
							case 1:
								mines.m_Value1 &= (UInt64.MaxValue ^ (3UL << shft));
								switch (newOwner)
								{
									case PlayerType.Hero2: mines.m_Value1 |= 1UL << shft; break;
									case PlayerType.Hero3: mines.m_Value1 |= 2UL << shft; break;
									case PlayerType.Hero4: mines.m_Value1 |= 3UL << shft; break;
								}
								break;
							default:
								mines.m_Value0 &= (UInt64.MaxValue ^ (3UL << shft));
								switch (newOwner)
								{
									case PlayerType.Hero2: mines.m_Value0 |= 1UL << shft; break;
									case PlayerType.Hero3: mines.m_Value0 |= 2UL << shft; break;
									case PlayerType.Hero4: mines.m_Value0 |= 3UL << shft; break;
								}
								break;
						}
					}
				}
			}
			return mines;
		}

		public int Count(PlayerType player)
		{
			return Count(player, IndexMax);
		}
		public int Count(PlayerType player, int length)
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

		public string ToString(int length) { return MineOwnership.ToString(this, length); }
		public override string ToString() { return ToString(IndexMax); }

		/// <summary>Creates mine ownership from tiles.</summary>
		public IMineOwnership UpdateFromTiles(string tiles)
		{
			IMineOwnership mines = MineOwnership64.Empty;
			return MineOwnership.UpdateFromTiles(mines, tiles);
		}

		public static IMineOwnership Create(params int[] mines)
		{
			return Create(mines.Select(mine => (PlayerType)mine).ToArray());
		}

		public static IMineOwnership Create(params PlayerType[] mines)
		{
			IMineOwnership ownership = MineOwnership64.Empty;

			for (int index = 0; index < mines.Length; index++)
			{
				if (mines[index] != PlayerType.None)
				{
					ownership = ownership.Set(index, mines[index]);
				}
			}
			return ownership;
		}

		public override int GetHashCode()
		{
			return m_Index.GetHashCode() ^ m_Value0.GetHashCode() ^ m_Value1.GetHashCode();
		}
		
		public override bool Equals(object obj) { return base.Equals(obj); }
		public bool Equals(IMineOwnership other) { return Equals((MineOwnership64)other); }
		public bool Equals(MineOwnership64 other)
		{
			return
				this.m_Index == other.m_Index &&
				this.m_Value0 == other.m_Value0 &&
				this.m_Value1 == other.m_Value1;
		}

		public static bool operator ==(MineOwnership64 l, MineOwnership64 r)
		{
			return l.Equals(r);
		}
		public static bool operator !=(MineOwnership64 l, MineOwnership64 r)
		{
			return !(l == r);
		}
	}
}
