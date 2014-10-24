using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Vindinium.DrunkenViking
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SafePath :  IEquatable<SafePath>
	{
		public static readonly SafePath NoPath = new SafePath(0, 1, 0, MoveDirection.x);

		public SafePath(int mines, int turns, int profit, params MoveDirection[] directions)
		{
			if (turns < 1) { throw new ArgumentOutOfRangeException("turns", "At least on turn should be involved."); }
			if (directions == null || directions.Length == 0) { throw new ArgumentNullException("directions"); }

			this.Mines = mines;
			this.Turns = turns;
			this.Profit = profit;
			this.Directions = new MoveDirections(directions);
		}

		public int Mines { get; protected set; }
		public int Turns { get; protected set; }
		public int Profit { get; protected set; }
		public MoveDirections Directions { get; protected set; }

		[ExcludeFromCodeCoverage]
		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Mines: {0}, Profit: {1}, Turns: {2}, Directions: {3}",
					this.Mines,
					this.Profit,
					this.Turns,
					this.Directions.Equals(MoveDirections.None) ? "<none>" : String.Join(", ", this.Directions));
			}
		}
		public override string ToString() { return this.DebuggerDisplay; }

		public override int GetHashCode()
		{
			var hash = Directions.GetHashCode();
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
			return
				other != null &&
				this.Mines == other.Mines &&
				this.Turns == other.Turns &&
				this.Profit == other.Profit &&
				this.Directions.Equals(other.Directions);
		}

		public string Evaluation
		{
			get
			{
				return DebuggerDisplay;
			}
		}
	}
}
