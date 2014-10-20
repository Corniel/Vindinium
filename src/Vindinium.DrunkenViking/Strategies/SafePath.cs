using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Vindinium.DrunkenViking.Strategies
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SafePath : IComparable<SafePath>
	{
		public SafePath(MoveDirection[] directions, int turns, int profit)
		{
			this.Directions = directions;
			this.Turns = turns;
			this.Profit = profit;
		}

		public int Turns { get; protected set; }
		public int Profit { get; protected set; }
		public MoveDirection[] Directions { get; protected set; }

		public int CompareTo(SafePath other)
		{
			var compare = other.Profit.CompareTo(this.Profit);
			if (compare == 0)
			{
				compare = this.Turns.CompareTo(other.Turns);
			}
			return compare;
		}

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Profit: {0}, Directions: {1}",
					this.Profit,
					this.Directions == null ? "<none>" : String.Join(", ", this.Directions));
			}
		}
	}
}
