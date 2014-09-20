namespace Vindinium
{
	/// <summary>Represents the move direction.</summary>
	/// <remarks>
	///      -N (y)
	///       |
	///  - W--x--E + (x)
	///       |
	///      +S
	/// </remarks>
	public enum MoveDirection
	{
		/// <summary>Move North.</summary>
		N = 0,
		/// <summary>Move West.</summary>
		W = 1,
		/// <summary>Move South.</summary>
		S = 2,
		/// <summary>Move East.</summary>
		E = 3,
		/// <summary>Stay.</summary>
		x = 4,
	}

	/// <summary>Extra ove directions.</summary>
	public static class MoveDirections
	{
		/// <summary>Gets all directions.</summary>
		public static readonly MoveDirection[] All = new MoveDirection[] { MoveDirection.N, MoveDirection.W, MoveDirection.S, MoveDirection.E, MoveDirection.x };
	}
}
