using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Vindinium
{
	/// <summary>Extra move directions.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct MoveDirections
	{
		/// <summary>Gets all directions.</summary>
		public static readonly MoveDirection[] All = new MoveDirection[] { MoveDirection.N, MoveDirection.W, MoveDirection.S, MoveDirection.E, MoveDirection.x };

		public static readonly MoveDirections None = default(MoveDirections);

		public static readonly MoveDirections N = new MoveDirections(MoveDirection.N);
		public static readonly MoveDirections W = new MoveDirections(MoveDirection.W);
		public static readonly MoveDirections S = new MoveDirections(MoveDirection.S);
		public static readonly MoveDirections E = new MoveDirections(MoveDirection.E);
		public static readonly MoveDirections x = new MoveDirections(MoveDirection.x);

		public MoveDirections(params MoveDirection[] directions)
		{
			m_Value = MoveDirections.ToByte(directions);
		}
		public MoveDirections(IEnumerable<MoveDirection> directions)
		{
			m_Value = MoveDirections.ToByte(directions);
		}

		private byte m_Value;

		public int Length { get { return Bits.Count(m_Value); } }

		public bool IsEmpty() { return m_Value == default(byte); }

		public bool Contains(MoveDirection direction)
		{
			return (m_Value & (1 << (int)direction)) != 0;
		}

		public MoveDirection[] ToArray()
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

		private static byte ToByte(IEnumerable<MoveDirection> directions)
		{
			byte val = 0;
			foreach (var direction in directions)
			{
				val |= (byte)(1 << (int)direction);
			}
			return val;
		}
	}
}
