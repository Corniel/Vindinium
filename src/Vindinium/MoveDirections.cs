using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium
{
	/// <summary>Extra move directions.</summary>
	public struct MoveDirections
	{
		/// <summary>Gets all directions.</summary>
		public static readonly MoveDirection[] All = new MoveDirection[] { MoveDirection.N, MoveDirection.W, MoveDirection.S, MoveDirection.E, MoveDirection.x };

		public static readonly MoveDirections None = default(MoveDirections);

		private byte m_Value;

		public MoveDirections(params MoveDirection[] directions)
		{
			m_Value = 0;
			foreach (var direction in directions)
			{
				m_Value |= (byte)(1 << (int)direction);
			}
		}

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

		public override int GetHashCode() { return m_Value; }
		public override bool Equals(object obj) { return base.Equals(obj); }
	}
}
