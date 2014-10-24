using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking
{
	public class SafePathComparer: IComparer<SafePath>
	{
		public SafePathComparer(int maxTurns)
		{
			this.MaxTurns = maxTurns;
		}
		public int MaxTurns { get; protected set; }

		/// <summary>Compares to safe paths.</summary>
		/// <returns>
		/// -1 if x is better, 0 if equal good, +1 if y is better.
		/// </returns>
		/// <remarks>
		/// Calculates a maximum result based on a potential score.
		/// </remarks>
		public int Compare(SafePath x, SafePath y)
		{
			var scoreX = x.Profit + (this.MaxTurns - x.Turns) * x.Mines;
			var scoreY = y.Profit + (this.MaxTurns - y.Turns) * y.Mines;

			var compare = scoreY.CompareTo(scoreX);
			if (compare == 0)
			{
				compare = x.Turns.CompareTo(y.Turns);
			}

			return compare;
		}
	}
}
