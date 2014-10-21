using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vindinium.DrunkenViking.Strategies
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SafePath :  IEquatable<SafePath>
	{
		public SafePath(int mines, int turns, int profit, params MoveDirection[] directions)
		{
			if (turns < 1) { throw new ArgumentOutOfRangeException("turns", "At least on turn should be involved."); }
			if (directions == null || directions.Length == 0) { throw new ArgumentNullException("directions"); }

			this.Mines = mines;
			this.Turns = turns;
			this.Profit = profit;
			this.Directions = directions;
		}

		public int Mines { get; protected set; }
		public int Turns { get; protected set; }
		public int Profit { get; protected set; }
		public MoveDirection[] Directions { get; protected set; }

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Mines: {0}, Profit: {1}, Turns: {2}, Directions: {3}",
					this.Mines,
					this.Profit,
					this.Turns,
					this.Directions == null ? "<none>" : String.Join(", ", this.Directions));
			}
		}
		public override string ToString() { return this.DebuggerDisplay; }

		public override int GetHashCode()
		{
			var hash = 0;
			foreach (var d in this.Directions)
			{
				hash |= 1 << (int)d;
			}
			hash |= Turns << 5;
			hash |= Profit << 10;
			hash |= Mines << 24;
			return hash;
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as SafePath);
		}
		public bool Equals(SafePath other)
		{

			if (other != null && 
				this.Mines == other.Mines &&
				this.Turns == other.Turns && 
				this.Profit == other.Profit && 
				this.Directions.Length == other.Directions.Length)
			{
				foreach (var d in this.Directions)
				{
					if (!other.Directions.Contains(d)) { return false; }
				}
				return true;
			}
			return false;
		}
	}
}
