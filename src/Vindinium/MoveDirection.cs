using System.Collections.Generic;

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

	/// <summary>Extra move directions.</summary>
	public static class MoveDirectionExtensions
	{
		/// <summary>Gets the label of the move direction.</summary>
		public static string ToLabel(this MoveDirection move) { return Label[move]; }

		/// <summary>Gets the labels for the move directions.</summary>
		public static readonly Dictionary<MoveDirection, string> Label = new Dictionary<MoveDirection, string>() { 
			{ MoveDirection.N, "North" },
			{ MoveDirection.S, "South" },
			{ MoveDirection.W, "West" },
			{ MoveDirection.E, "East" },
			{ MoveDirection.x, "Stay" },
		};
	}
}
