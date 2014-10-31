using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Vindinium
{
	[DebuggerDisplay("{ToString()}")]
	public struct MineOwnership128 : IMineOwnership, IEquatable<MineOwnership128>
	{
		public const int IndexMax = 128;
		public static readonly MineOwnership128 Empty = default(MineOwnership128);

		private MineOwnership64 m_Value0;
		private MineOwnership64 m_Value1;

		public PlayerType this[int index]
		{
			get
			{
				switch (index >> 6)
				{
					case 01: return m_Value1[63 & index];
					default: return m_Value0[index];
				}
			}
		}

		public IMineOwnership Set(int index, PlayerType player)
		{
			var mines = new MineOwnership128()
			{
				m_Value0 = m_Value0,
				m_Value1 = m_Value1,
			};
			switch (index >> 6)
			{
				case 01: mines.m_Value1 = (MineOwnership64)mines.m_Value1.Set(63 & index, player); break;
				default: mines.m_Value0 = (MineOwnership64)mines.m_Value0.Set(index, player); break;
			}
			return mines;
		}
		public IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner)
		{
			return ChangeOwnership(curOwner, newOwner, IndexMax);
		}
		public IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner, int mineLength)
		{
			var mines = new MineOwnership128()
			{
				m_Value0 = (MineOwnership64)m_Value0.ChangeOwnership(curOwner, newOwner, Math.Min(mineLength, MineOwnership64.IndexMax)),
				m_Value1 = m_Value1,
			};
			if (mineLength > 64)
			{
				mines.m_Value1 = (MineOwnership64)m_Value1.ChangeOwnership(curOwner, newOwner, mineLength - 64);
			}
			return mines;
		}

		public int Count(PlayerType player)
		{
			return Count(player, IndexMax);
		}
		public int Count(PlayerType player, int length)
		{
			var count = m_Value0.Count(player, Math.Min(length, MineOwnership64.IndexMax));
			if (length > 64)
			{
				count += m_Value1.Count(player, length - 64);
			}
			return count;
		}

		public string ToString(int length) { return MineOwnership.ToString(this, length); }
		public override string ToString() { return ToString(IndexMax); }

		/// <summary>Creates mine ownership from tiles.</summary>
		public IMineOwnership UpdateFromTiles(string tiles)
		{
			IMineOwnership mines = MineOwnership128.Empty;
			return MineOwnership.UpdateFromTiles(mines, tiles);
		}

		public static IMineOwnership Create(params int[] mines)
		{
			return Create(mines.Select(mine => (PlayerType)mine).ToArray());
		}

		public static IMineOwnership Create(params PlayerType[] mines)
		{
			IMineOwnership ownership = MineOwnership128.Empty;

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
			return m_Value0.GetHashCode() ^ m_Value1.GetHashCode();
		}

		public override bool Equals(object obj) { return base.Equals(obj); }
		public bool Equals(MineOwnership128 other)
		{
			return this.m_Value0 == other.m_Value0 && this.m_Value1 == other.m_Value1;
		}

		public static bool operator ==(MineOwnership128 l, MineOwnership128 r)
		{
			return l.Equals(r);
		}
		public static bool operator !=(MineOwnership128 l, MineOwnership128 r)
		{
			return !(l == r);
		}
	}
}
